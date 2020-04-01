using System;
using System.Collections.Generic;
using System.Linq;

namespace Katas.Console._01.FSM
{
    public class TCP
    {
        public static string TraverseStates(string[] events)
        {
            //TODO: validate input events are valid events, or use TryParse() instead of Parse()
            var state = State.CLOSED; // initial state, always
            List<ActionEventState> actionEventStates = GetActions();

            foreach (var evv in events)
            {
                var ev = (Event)Enum.Parse(typeof(Event), evv);
                var aev = actionEventStates.Where(a => a.InitialState == state && a.Event == ev)
                                           .FirstOrDefault();

                if (aev != null) state = aev.NewState;
                else return "ERROR";
            }

            return state.ToString();
        }

        private static List<ActionEventState> GetActions()
        {
            return new List<ActionEventState>()
            {
                new ActionEventState { InitialState = State.CLOSED, Event = Event.APP_PASSIVE_OPEN, NewState = State.LISTEN },
                new ActionEventState { InitialState = State.CLOSED, Event = Event.APP_ACTIVE_OPEN, NewState = State.SYN_SENT },
                new ActionEventState { InitialState = State.LISTEN, Event = Event.RCV_SYN, NewState = State.SYN_RCVD },
                new ActionEventState { InitialState = State.LISTEN, Event = Event.APP_SEND, NewState = State.SYN_SENT },
                new ActionEventState { InitialState = State.LISTEN, Event = Event.APP_CLOSE, NewState = State.CLOSED },
                new ActionEventState { InitialState = State.SYN_RCVD, Event = Event.APP_CLOSE, NewState = State.FIN_WAIT_1 },
                new ActionEventState { InitialState = State.SYN_RCVD, Event = Event.RCV_ACK, NewState = State.ESTABLISHED },
                new ActionEventState { InitialState = State.SYN_SENT, Event = Event.RCV_SYN, NewState = State.SYN_RCVD },
                new ActionEventState { InitialState = State.SYN_SENT, Event = Event.RCV_SYN_ACK, NewState = State.ESTABLISHED },
                new ActionEventState { InitialState = State.SYN_SENT, Event = Event.APP_CLOSE, NewState = State.CLOSED },
                new ActionEventState { InitialState = State.ESTABLISHED, Event = Event.APP_CLOSE, NewState = State.FIN_WAIT_1 },
                new ActionEventState { InitialState = State.ESTABLISHED, Event = Event.RCV_FIN, NewState = State.CLOSE_WAIT },
                new ActionEventState { InitialState = State.FIN_WAIT_1, Event = Event.RCV_FIN, NewState = State.CLOSING },
                new ActionEventState { InitialState = State.FIN_WAIT_1, Event = Event.RCV_FIN_ACK, NewState = State.TIME_WAIT },
                new ActionEventState { InitialState = State.FIN_WAIT_1, Event = Event.RCV_ACK, NewState = State.FIN_WAIT_2 },

                new ActionEventState { InitialState = State.CLOSING, Event = Event.RCV_ACK, NewState = State.TIME_WAIT },
                new ActionEventState { InitialState = State.FIN_WAIT_2, Event = Event.RCV_FIN, NewState = State.TIME_WAIT },
                new ActionEventState { InitialState = State.TIME_WAIT, Event = Event.APP_TIMEOUT, NewState = State.CLOSED },
                new ActionEventState { InitialState = State.CLOSE_WAIT, Event = Event.APP_CLOSE, NewState = State.LAST_ACK },
                new ActionEventState { InitialState = State.LAST_ACK, Event = Event.RCV_ACK, NewState = State.CLOSED }
            };
        }
    }

    public class ActionEventState
    {
        public State InitialState { get; set; }
        public Event Event { get; set; }
        public State NewState { get; set; }
    }

    public enum State
    {
        CLOSED, LISTEN, SYN_SENT, SYN_RCVD, ESTABLISHED, CLOSE_WAIT, LAST_ACK, FIN_WAIT_1, FIN_WAIT_2, CLOSING, TIME_WAIT
    }

    public enum Event
    {
        APP_PASSIVE_OPEN, APP_ACTIVE_OPEN, APP_SEND, APP_CLOSE, APP_TIMEOUT, RCV_SYN, RCV_ACK, RCV_SYN_ACK, RCV_FIN, RCV_FIN_ACK
    }
}
