public enum DirectionInfo
{
	// 명명 규칙 <방향>
	// Straight = 직선 ↓
	// Left		= 좌측으로 꺾음(자동차 시점에서의 우회전) ↲
	// Right	= 우측으로 꺾음(자동차 시점에서의 좌회전) ↳
	// Both		= 좌우측 모두 존재 ↯

	// 명명 규칙 <enum>
	// -> "(출구방향)And(존재하는방향)"
	// -> 1 ~ 15 출구가 직선
	// -> 16 ~ 31 출구가 좌측
	// -> 32 ~ 47 출구가 우측
	// -> 48 ~ 출구가 없음 & 예외


	Straight = 0,
	StraightWithRight = 1,
	StraightWithLeft = 2,
	StraightWithBoth = 3,

	Left = 16,
	LeftWithRight = 17,
	LeftWithBoth = 19,

	Right = 32,
	RightWithLeft = 34,
	RightWithBoth = 35,

	None = 48,
}
