// ––‒–‒––––––‒–‒‒‒––‒––––––‒––––‒––‒‒–
// Коммерческая лицензия подписчика
// (c) 2023-2025 Leopotam <leopotam@yandex.ru>
// ––‒–‒––––––‒–‒‒‒––‒––––––‒––––‒––‒‒–

using UnityEngine;
using UnityEngine.EventSystems;

namespace Leopotam.EcsProto.Unity.Ugui {
    public struct UnityUguiClickEvent {
        public string SenderName;
        public GameObject Sender;
        public Vector2 Position;
        public PointerEventData.InputButton Button;
    }
}
