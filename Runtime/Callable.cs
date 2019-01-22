using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameplayIngredients
{
    public abstract class Callable : MonoBehaviour, ICallable
    {
        public string Name;

        public Callable()
        {
            Name = GetType().Name;
        }

        public abstract void Execute();
        public abstract new string ToString();

        public static void Call(Callable[] calls)
        {
            foreach (var call in calls)
            {
                if(call != null)
                    call.Execute();
                else
                    Debug.LogError("Cannot execute call: Null or Missing");

            }
        }

        public static void Call(Callable call)
        {
            if (call != null)
                call.Execute();
            else
                Debug.LogError("Cannot execute call: Null or Missing");
        }
    }
}

