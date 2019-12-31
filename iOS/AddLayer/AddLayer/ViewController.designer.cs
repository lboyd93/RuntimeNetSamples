// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace AddLayer
{
    [Register ("ViewController")]
    partial class ViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton AddLayer { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton ClearButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField InputServiceURL { get; set; }

        [Action ("AddLayer_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void AddLayer_TouchUpInside (UIKit.UIButton sender);

        [Action ("ClearButton_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void ClearButton_TouchUpInside (UIKit.UIButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (AddLayer != null) {
                AddLayer.Dispose ();
                AddLayer = null;
            }

            if (ClearButton != null) {
                ClearButton.Dispose ();
                ClearButton = null;
            }

            if (InputServiceURL != null) {
                InputServiceURL.Dispose ();
                InputServiceURL = null;
            }
        }
    }
}