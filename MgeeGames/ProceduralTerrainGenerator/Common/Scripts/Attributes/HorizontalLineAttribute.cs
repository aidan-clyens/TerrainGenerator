using System;
using UnityEngine;

namespace CustomAttributes {

    [AttributeUsage(AttributeTargets.Field, Inherited = true)]
    public class HorizontalLineAttribute : PropertyAttribute {
        public HorizontalLineAttribute() {

        }
    }
}
