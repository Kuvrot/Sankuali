// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org/ & https://stride3d.net) and Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.
using System;
using Stride.Core;
using Stride.Core.Mathematics;
using Stride.Engine;
using Stride.Engine.Events;
using Stride.Physics;
using System.Collections.Generic;
using Stride.Input;
using Furia.Stats;

namespace Furia.Player
{
    public class PlayerController : SyncScript
    {
        [Display("Run Speed")]
        public float MaxRunSpeed { get; set; } = 5;

        public static readonly EventKey<float> RunSpeedEventKey = new EventKey<float>();

        // This component is the physics representation of a controllable character
        private CharacterComponent character;
        private FootstepsSystem footstepsSystem;

        private readonly EventReceiver<Vector3> moveDirectionEvent = new EventReceiver<Vector3>(PlayerInput.MoveDirectionEventKey);

        [Display("WeaponInventory")]
        
        public override void Start()
        {
            base.Start();

            // Will search for an CharacterComponent within the same entity as this script
            character = Entity.Get<CharacterComponent>();
            if (character == null) throw new ArgumentException("Please add a CharacterComponent to the entity containing PlayerController!");

            footstepsSystem = Entity.Get<FootstepsSystem>();
        }
        
        /// <summary>
        /// Called on every frame update
        /// </summary>
        public override void Update()
        {
            Move();
        }

        private void Move()
        {
            // Character speed
            Vector3 moveDirection = Vector3.Zero;
            moveDirectionEvent.TryReceive(out moveDirection);

            character.SetVelocity(moveDirection * MaxRunSpeed);

            footstepsSystem?.setWalking(moveDirection != Vector3.Zero);

            // Broadcast normalized speed
            RunSpeedEventKey.Broadcast(moveDirection.Length());
        }
    }
}
