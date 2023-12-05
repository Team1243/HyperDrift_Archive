using System;
 
namespace PXG.UIElements {
    /// <summary>
    /// <para>Represents a <see cref="FillableBar"/>'s fill direction (horizontal or vertical orientations only).</para>
    /// </summary>
    [Serializable]
    public enum FillDirection {
        LeftToRight = 0,
        RightToLeft = 1,
        BottomToTop = 2,
        TopToBottom = 3
    }
}
 
