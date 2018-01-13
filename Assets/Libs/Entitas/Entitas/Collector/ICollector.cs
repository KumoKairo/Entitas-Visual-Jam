using System.Collections.Generic;

namespace Entitas {

    public interface ICollector {

        int count { get; }

		void Activate();
		void Deactivate();
		void ClearCollectedEntities();

        IEnumerable<TCast> GetCollectedEntities<TCast>() where TCast : class, IEntity;
        HashSet<IEntity> collectedEntities { get; }
    }
}
