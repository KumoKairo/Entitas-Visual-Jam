using System;

namespace Entitas {

    public static class CollectorContextExtension {

        /// Creates a Collector.
        public static ICollector CreateCollector(
            this IContext context, IMatcher matcher) {

            return context.CreateCollector(new TriggerOnEvent(matcher, GroupEvent.Added));
        }

        /// Creates a Collector.
        public static ICollector CreateCollector(
            this IContext context, params TriggerOnEvent[] triggers) {

            var groups = new IGroup[triggers.Length];
            var groupEvents = new GroupEvent[triggers.Length];

            for (int i = 0; i < triggers.Length; i++) {
                groups[i] = context.GetGroup(triggers[i].matcher);
                groupEvents[i] = triggers[i].groupEvent;
            }

            return new Collector(groups, groupEvents);
        }
    }
}
