// ––‒–‒––––––‒–‒‒‒––‒––––––‒––––‒––‒‒–
// Коммерческая лицензия подписчика
// (c) 2023-2025 Leopotam <leopotam@yandex.ru>
// ––‒–‒––––––‒–‒‒‒––‒––––––‒––––‒––‒‒–

using UnityEngine;
#if ENABLE_IL2CPP
using Unity.IL2CPP.CompilerServices;
#endif

namespace Leopotam.EcsProto.Unity.Physics3D {
#if ENABLE_IL2CPP
    [Il2CppSetOption (Option.NullChecks, false)]
    [Il2CppSetOption (Option.ArrayBoundsChecks, false)]
#endif
    public sealed class UnityPhysics3DCollisionEnterAction : ProtoUnityAction<UnityPhysics3DCollisionEnterEvent> {
        public void OnCollisionEnter (Collision collision) {
            if (IsValidForEvent ()) {
                ref var msg = ref NewEvent ();
                msg.SenderName = SenderName ();
                msg.Sender = Sender ();
                msg.Collider = collision.collider;
                msg.Velocity = collision.relativeVelocity;
                var cp = collision.GetContact (0);
                msg.Point = cp.point;
                msg.Normal = cp.normal;
            }
        }
    }
}
