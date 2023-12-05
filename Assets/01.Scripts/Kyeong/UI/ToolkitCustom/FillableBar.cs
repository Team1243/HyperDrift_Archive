using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;
 
namespace PXG.UIElements {
    /// <summary>
    /// Represents a <see cref="VisualElement"/> that may be partially filled. This is useful for HP bars, for example.
    /// </summary>
    public class FillableBar : VisualElement {
        //Must create a UxmlFactory in order to be exposed to UXML and UI Builder!
        public new class UxmlFactory : UxmlFactory<FillableBar, UxmlTraits> { }
 
        //Use this to expose additional custom UXML attributes!
        public new class UxmlTraits : VisualElement.UxmlTraits {
            private UxmlFloatAttributeDescription fillAmount = new UxmlFloatAttributeDescription() {
                name = "fill-amount", //The name used for an actual UXML attribute (written in a .uxml file)
                defaultValue = 1
            };
            private UxmlEnumAttributeDescription<FillDirection> fillDirection = new UxmlEnumAttributeDescription<FillDirection>() {
                name = "fill-direction",
                defaultValue = FillDirection.LeftToRight
            };
 
 
            public override void Init(VisualElement visualElement, IUxmlAttributes bag, CreationContext context) {
                base.Init(visualElement, bag, context);
 
                FillableBar element = visualElement as FillableBar;
 
                element.FillAmount = fillAmount.GetValueFromBag(bag, context);
                element.FillDirection = fillDirection.GetValueFromBag(bag, context);
            }
        }
 
        public float FillAmount { get; set; }
        public FillDirection FillDirection { get; set; }
 
        public FillableBar() {
            generateVisualContent = GenerateVisualContent;
        }
 
        private void GenerateVisualContent(MeshGenerationContext context) {
            IResolvedStyle resolvedStyle = this.resolvedStyle;
            MeshWriteData data = context.Allocate(4, 6, resolvedStyle.backgroundImage.texture);
 
            Rect localBound = this.localBound;
            Color32 tintColor = resolvedStyle.unityBackgroundImageTintColor;
 
            //WARNING: VisualElement local space is (0, 0) at top-left, and goes +XY right-down (respectively).
            float leftPos = localBound.xMin;
            float rightPos = localBound.xMax;
            float bottomPos = localBound.yMax;
            float topPos = localBound.yMin;
 
            float fillAmount = math.saturate(FillAmount); //Saturate => keep it in range [0, 1]
 
            //These are coordinates using bottom-left (0, 0) like UVs
            float leftUV = 0;
            float rightUV = 1;
            float bottomUV = 0;
            float topUV = 1;
 
            switch (FillDirection) {
                case FillDirection.LeftToRight:
                    rightUV = fillAmount;
                    rightPos = math.lerp(leftPos, rightPos, fillAmount);
                    break;
                case FillDirection.RightToLeft:
                    leftUV = 1 - fillAmount;
                    leftPos = math.lerp(rightPos, leftPos, fillAmount);
                    break;
                case FillDirection.BottomToTop:
                    topUV = fillAmount;
                    topPos = math.lerp(bottomPos, topPos, fillAmount);
                    break;
                case FillDirection.TopToBottom:
                    bottomUV = 1 - fillAmount;
                    bottomPos = math.lerp(topPos, bottomPos, fillAmount);
                    break;
            }
 
            data.SetNextVertex(new Vertex() {
                position = new Vector3(leftPos, bottomPos),
                tint = tintColor,
                uv = new Vector2(leftUV, bottomUV)
            });
 
            data.SetNextVertex(new Vertex() {
                position = new Vector3(leftPos, topPos),
                tint = tintColor,
                uv = new Vector2(leftUV, topUV)
            });
 
            data.SetNextVertex(new Vertex() {
                position = new Vector3(rightPos, bottomPos),
                tint = tintColor,
                uv = new Vector2(rightUV, bottomUV)
            });
 
            data.SetNextVertex(new Vertex() {
                position = new Vector3(rightPos, topPos),
                tint = tintColor,
                uv = new Vector2(rightUV, topUV)
            });
           
            data.SetNextIndex(0);
            data.SetNextIndex(1);
            data.SetNextIndex(2);
            data.SetNextIndex(3);
            data.SetNextIndex(2);
            data.SetNextIndex(1);
        }
    }
}
 