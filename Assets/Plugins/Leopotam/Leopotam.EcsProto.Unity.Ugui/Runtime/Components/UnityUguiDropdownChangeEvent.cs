// ––‒–‒––––––‒–‒‒‒––‒––––––‒––––‒––‒‒–
// Коммерческая лицензия подписчика
// (c) 2023-2025 Leopotam <leopotam@yandex.ru>
// ––‒–‒––––––‒–‒‒‒––‒––––––‒––––‒––‒‒–

using TMPro;

namespace Leopotam.EcsProto.Unity.Ugui {
    public struct UnityUguiDropdownChangeEvent {
        public string SenderName;
        public TMP_Dropdown Sender;
        public int Value;
    }
}
