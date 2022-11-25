using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
using UnityEditor.UIElements;

public class BlackboardElement : VisualElement {

    public new class UxmlFactory : UxmlFactory<BlackboardElement, VisualElement.UxmlTraits> {}

    public BlackboardElement() {
        style.flexGrow = 1;
    }
}
