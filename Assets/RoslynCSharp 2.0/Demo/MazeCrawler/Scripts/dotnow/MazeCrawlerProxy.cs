#if DOTNOW && ENABLE_IL2CPP
using RoslynCSharp.Demo;
using System.Reflection;
using UnityEngine;

namespace dotnow.Interop
{
    /// <summary>
    /// Required by dotnow to allow inheritance of MazeCrawler from compiled code.
    /// </summary>
    [CLRProxyBinding(typeof(MazeCrawler))]
    public class MazeCrawlerProxy : MazeCrawler, ICLRProxy
    {
        // Private
        private CLRInstance instance;
        private MethodInfo decideDirectionMethod = null;

        // Properties
        public CLRInstance Instance => instance;

        // Methods
        public void InitializeProxy(AppDomain domain, CLRInstance instance)
        {
            this.instance = instance;
            this.decideDirectionMethod = instance.Type.GetMethod(nameof(DecideDirection), BindingFlags.Public | BindingFlags.Instance);
        }

        public override MazeDirection DecideDirection(Vector2Int position, bool canMoveLeft, bool canMoveRight, bool canMoveUp, bool canMoveDown)
        {
            return (MazeDirection)decideDirectionMethod.Invoke(instance, new object[] { position, canMoveLeft, canMoveRight, canMoveUp, canMoveDown });
        }
    }
}
#endif
