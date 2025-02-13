using System;
using UnityEngine.Events;

namespace EventSystem
{
    /// <summary>
    /// Interface for game events.
    /// </summary>
    public interface IGameEvent
    {
        /// <summary>
        /// Adds a delegate to the event list.
        /// </summary>
        /// <param name="action">The delegate to add.</param>
        public void Add(Delegate action);

        /// <summary>
        /// Removes a delegate from the event list.
        /// </summary>
        /// <param name="action">The delegate to remove.</param>
        public void Remove(Delegate action);
        
        /// <summary>
        /// Invokes the event.
        /// </summary>
        public void Invoke();
    }
    
    /// <summary>
    /// Game event with no parameters.
    /// </summary>
    public class GameEvent : IGameEvent
    {
        /// <summary>
        /// The event.
        /// </summary>
        public UnityAction Event;

        /// <summary>
        /// Adds a delegate to the event list.
        /// </summary>
        /// <param name="action">The delegate to add.</param>
        public void Add(UnityAction action)
        {
            Event += action;
        }

        /// <summary>
        /// Adds a delegate to the event list.
        /// </summary>
        /// <param name="action">The delegate to add.</param>
        public void Add(Delegate action)
        {
            if (action is UnityAction unityAction)
            {
                Event += unityAction;
            }
        }

        /// <summary>
        /// Removes a delegate from the event list.
        /// </summary>
        /// <param name="action">The delegate to remove.</param>
        public void Remove(UnityAction action)
        {
            Event -= action;
        }

        /// <summary>
        /// Removes a delegate from event list.
        /// </summary>
        /// <param name="action">The delegate to remove.</param>
        public void Remove(Delegate action)
        {
            if (action is UnityAction unityAction)
            {
                Event -= unityAction;
            }
        }

        /// <summary>
        /// Invokes the event.
        /// </summary>
        public void Invoke()
        {
            Event?.Invoke();
        }
    }

    /*
     * We purposefully only provide an interface with a single 
     * generic type to encourage clients to define custom data
     * types whose intent is more clear than that of their 
     * primative counterparts.
     * 
     * Example: Which is immediately more clear?
     * 
     * 1. FireStarted?.Invoke(true, 8, "Bang!"); 
     * 
     *         -- or --
     * 
     * 2. struct FireData
     *    {
     *       public bool immediate;
     *       public float burstCount;
     *       public string caption;
     *    }
     * 
     *    FireStarted?.Invoke(FireData);
     *    
     * Clearly, option 2 gives much more detail as to the 
     * purpose of the parameters. This is exactly the same for 
     * all number of parameters INCLUDING 1.
     */

    /// <summary>
    /// Game event with one parameter.
    /// </summary>
    /// <typeparam name="T">The type of the parameter to pass in.</typeparam>
    public class GameEvent<T> : IGameEvent
    {
        /// <summary>
        /// The event.
        /// </summary>
        public UnityAction<T> Event;
        
        /// <summary>
        /// The value of the most recent parameter this event was invoked with.
        /// </summary>
        public T Latest { get; private set; }

        /// <summary>
        /// Adds a delegate to event list.
        /// </summary>
        /// <param name="action">The delegate to add.</param>
        public void AddListener(UnityAction<T> action)
        {
            Event += action;
        }

        /// <summary>
        /// Adds a delegate to the event list.
        /// </summary>
        /// <param name="action">The delegate to add.</param>
        public void Add(Delegate action)
        {
            if (action is UnityAction<T> unityAction)
            {
                Event += unityAction;
            }
        }

        /// <summary>
        /// Removes a delegate from the event list.
        /// </summary>
        /// <param name="action">The delegate to remove.</param>
        public void RemoveListener(UnityAction<T> action)
        {
            Event -= action;
        }

        /// <summary>
        /// Remove a delegate from the event list.
        /// </summary>
        /// <param name="action">The delegate to remove.</param>
        public void Remove(Delegate action)
        {
            if (action is UnityAction<T> unityAction)
            {
                Event -= unityAction;
            }
        }
        
        /// <summary>
        /// Invokes the event
        /// </summary>
        /// <param name="arg">The arg to pass to event listeners.</param>
        public void Invoke(T arg)
        {
            Latest = arg;
            Event?.Invoke(arg);
        }

        /// <summary>
        /// Invoke using the most recent value passed to the event.
        /// </summary>
        public void Invoke()
        {
            if (Latest == null)
            {
                throw new Exception("You cannot call Invoke() before Invoke(arg). Did you mean to pass data with this event?");
            }
            Invoke(Latest);
        }
    }
}


