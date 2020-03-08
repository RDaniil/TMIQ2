using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tmiq2.Abilities
{
    public abstract class Ability : MonoBehaviour
    {
        public abstract IEnumerator Cast();
    }

}