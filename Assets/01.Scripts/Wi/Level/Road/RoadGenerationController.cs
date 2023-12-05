using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 맵 생성 스크립트
/// </summary>
public class RoadGenerationController : MonoBehaviour
{
	[Header("<References>")]
	[SerializeField] private Transform player;
	[SerializeField] private RoadCollectionSO collection;

	private RoadBehind behind;
	private RoadPoolManager poolManager;

	[Header("<Settings>")]
	[Tooltip("앞쪽으로 생성할 도로의 개수")]
	[SerializeField] private int foreSectionRange;
	[Tooltip("뒤쪽으로 남겨둘 도로의 개수")]
	[SerializeField] private int rearSectionRange;
	[Tooltip("같은 도로가 연속으로 나올 수 없다")]
	[SerializeField] private bool unrepeatableRoad = false;
	[Tooltip("직선 도로가 연속으로 나올 수 없다")]
	[SerializeField] private bool unrepeatableStraight = true;

	[Header("<Flags>")]
	private List<RoadSection> currentSections = new (); // 현재 생성된 섹션들
	private RoadSection playerLocatedSection; // 현재 플레이어가 위치한 섹션
	private RoadSection lastCreatedSection; // 가장 최근 생성된 섹션
	[HideInInspector] public DirectionInfo RoadDirection { get; private set; } // 도로의 끝이 바라보는 방향

	private float areaZPos; // 범위의 최소 z위치
	private float roadZPos; // lastCreatedSection의 Exit의 z위치
	private int roadNumber = 0; // 도로의 번호, 생성될 때마다 1씩 상승

	private bool cannot = false;

	public event Action<RoadSection, int> OnCreateRoad; // 도로가 생성될 때 호출되는 이벤트

	private void Awake()
	{
		poolManager = new RoadPoolManager(collection, transform, 2);
		poolManager.MakePools();
	}

	private void Start()
	{
		// 시작된 후 시작 섹션을 만든다.
		RoadSection section = poolManager.Pop(collection.startSection.gameObject.name) as RoadSection;
		section.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
		AddToList(section);
		RoadDirection = currentSections[0].ExitDir;

		// 전방으로 생성해야 하는 도로의 개수 만큼 도로를 생성한다.
		int n = PlayerCurrentSectionIndex();
		if (n != -1)
		{
			while (ShouldCreateNewSection(n))
			{
				CreateNewSection();
			}
		}

		// 뒤로 가는 것을 방지하는 구조물을 만든다.
		behind = Instantiate(collection.roadBehind.gameObject).GetComponent<RoadBehind>();
		AlignBehind();
	}

	private void Update()
	{
		int n = PlayerCurrentSectionIndex();
		if (ShouldCreateNewSection(n))
		{
			CreateNewSection();
		}
		if (ShouldDeleteOldSection(n))
		{
			DeleteOldSection();
		}
		Debug.DrawLine(Vector3.forward * areaZPos, Vector3.zero, Color.red);
	}

	/// <summary>
	/// 플레이어가 어느 섹션 내에 있는지 확인하는 함수
	/// </summary>
	/// <returns>어떤 섹션 내에도 없을 시에는 -1을 반환</returns>
	private int PlayerCurrentSectionIndex()
	{
		if (currentSections.Count == 0) return -1;

		for (int i = 0; i < currentSections.Count; i++)
		{
			if (PositionInArea(currentSections[i], player.position))
			{
				if (currentSections[i] != playerLocatedSection)
				{
					playerLocatedSection = currentSections[i];
					//FitCameraFov();
				}
				return i;
			}
		}

		return -1;
	}

	/// <summary>
	/// 플레이어가 매개변수로 들어온 섹션 내에 존재하는지 확인하는 함수
	/// </summary>
	public bool PositionInArea(RoadSection section, Vector3 pos)
	{
		float left = section.transform.position.x - (section.Size.x / 2);
		float right = section.transform.position.x + (section.Size.x / 2);
		float top = section.transform.position.z + (section.Size.z / 2);
		float bottom = section.transform.position.z - (section.Size.z / 2);

		if (pos.x > left && pos.x < right && pos.z < top && pos.z > bottom)
		{
			return true;
		}

		return false;
	}

	#region 도로의 생성과 제거 조건을 확인

	/// <summary>
	/// 새로운 섹션을 생성해야 하는가
	/// </summary>
	/// <param name="n"></param>
	public bool ShouldCreateNewSection(int n)
	{
		if (n == -1) return false;

		return (currentSections.Count - 1 - foreSectionRange) < n;
	}

	/// <summary>
	/// 뒤의 섹션을 제거해야 하는가
	/// </summary>
	/// <param name="n"></param>
	public bool ShouldDeleteOldSection(int n)
	{
		if (n == -1) return false;

		return rearSectionRange < n;
	}

	#endregion

	#region 생성하기로 결정한 섹션이 조건에 맞는지 검사

	private bool IsAbleRoad(RoadSection newRoad)
	{
		if (areaZPos < roadZPos && RoadDirection == DirectionInfo.Straight) // 도로가 겹치는 것을 방지하는 코드
		{
			return (int)newRoad.ExitDir < 16;
		}
		else if (lastCreatedSection != null) // 도로가 겹치지 않을 경우에만, 도로 반복 및 직선 반복 검사
		{
			if (unrepeatableRoad) // 도로 반복 불가
			{
				bool isSame = lastCreatedSection.gameObject.name.Equals(newRoad.gameObject.name);
				if (isSame) // 같은 도로가 연속으로 나온 경우
					return false;
			}
			if (unrepeatableStraight) // 직선 반복 불가
			{
				bool isStraightAgain = (int)lastCreatedSection.ExitDir < 16 && (int)newRoad.ExitDir < 16;
				if (isStraightAgain) // 직선 도로가 연속으로 나온 경우
					return false;
			}
		}

		int newDir = (int)newRoad.ExitDir;
		switch ((int)RoadDirection / 16)
		{
			case 0: // 이전 도로가 직선일 경우
				return true;
			case 1: // 이전 도로가 좌측일 경우
				if (newDir / 16 != 1 && newDir % 16 != 2 && newDir % 16 != 3)
				{
					return true;
				}
				break;
			case 2: // 이전 도로가 우측일 경우
				if (newDir / 16 != 2 && newDir % 16 != 1 && newDir % 16 != 3)
				{
					return true;
				}
				break;
			default:
				return false;
		}
		return false;
	}

	#endregion

	#region 제거 및 생성 실행

	private void DeleteOldSection()
	{
		RoadSection sec = currentSections[0];
		currentSections.RemoveAt(0);
		poolManager.Push(sec);
		AlignBehind();
	}

	/// <summary>
	/// 새 도로 생성
	/// </summary>
	private void CreateNewSection()
	{
		if (cannot) return;
		RoadSection section;
		int loop = 0;
		while (true)
		{
			loop++;
			section = collection.sections[UnityEngine.Random.Range(0, collection.sections.Count)];

			if (IsAbleRoad(section))
			{
				section = poolManager.Pop(section.gameObject.name) as RoadSection;
				break;
			}

			if (loop == 1000)
			{
				Debug.LogError($"No Part for create new section. [{lastCreatedSection.gameObject.name}] [{section.gameObject.name}]");
				cannot = true;
				break;
			}
		}

		Align(section);
		AddToList(section);

		if ((int)RoadDirection < 16)
		{
			RoadDirection = section.ExitDir;
		}
		else
		{
			if ((int)section.ExitDir / 16 != 0)
				RoadDirection = DirectionInfo.Straight;
		}

		OnCreateRoad?.Invoke(section, roadNumber++);
	}

	private void AddToList(RoadSection section)
	{
		currentSections.Add(section);
		lastCreatedSection = section;
	}

	/// <summary>
	/// 매개변수로 입력된 섹션을 기존의 도로와 연결되도록 각도 및 위치 조정
	/// </summary>
	/// <param name="section">정렬할 섹션</param>
	private void Align(RoadSection section)
	{
		int dir = (int)RoadDirection / 16;
		float angle = dir == 0 ? 0 : dir == 1 ? 90 : -90;
		section.transform.rotation = Quaternion.Euler(0, angle, 0);

		Vector3 pos = section.transform.position - section.EnterPosition.position;
		section.transform.position = lastCreatedSection.ExitPosition.position + pos;

		float newAreaZPos = section.transform.position.z - (angle == 0 ? section.Size.z : section.Size.x) / 2;
		if (newAreaZPos < areaZPos)
			areaZPos = newAreaZPos;
		roadZPos = section.ExitPosition.position.z;

	}

	/// <summary>
	/// 뒤로 가는 것을 방지하는 섹션 정렬
	/// </summary>
	private void AlignBehind()
	{
		if (currentSections.Count <= 0 || behind is null) return;

		behind.transform.rotation = currentSections[0].transform.rotation;
		Vector3 position = currentSections[0].EnterPosition.position + behind.transform.position - behind.Center.position;
		behind.transform.position = position;
	}

	#endregion
}
