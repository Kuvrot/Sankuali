﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stride.Core.Mathematics;
using Stride.Input;
using Stride.Engine;
using Vortice.Vulkan;
using Stride.Rendering.Sprites;
using Stride.Graphics;
using SharpDX.Direct3D11;

namespace Furia.NPC.Animation
{
    public class Npc2dAnimationController : SyncScript
    {
        public byte animationSpeed = 12; // I have no idea how to describe how the speed works, but lesser the value, faster the animation
        public byte idleStartFrame = 0, idleEndFrame = 0;
        public byte attackStartFrame = 0 , attackEndFrame = 0;
        public byte walkStartFrame = 0, walkEndFrame = 0;
        public byte deathStartFrame = 0, deathEndFrame = 0;
        public byte hitStartFrame = 0, hitEndFrame = 0;

        private byte currentStartFrame = 0, currentEndFrame = 0;
        private float clock = 0;

        //Animation state machine
        /*
        -1 = death 
        0 = idle
        1 = walk
        2 = attack 
        */

        //Components
        private SpriteComponent spriteComponent;
        private SpriteFromSheet spriteSheet;

        public override void Start()
        {
            //This is how you are supposed to set sprite frames https://doc.stride3d.net/4.0/en/manual/sprites/use-sprites.html
            spriteComponent = Entity.Get<SpriteComponent>();
            spriteSheet = spriteComponent.SpriteProvider as SpriteFromSheet;
        }

        public override void Update()
        {
        }

        public void PlayIdleAnimation()
        {
            currentStartFrame = idleStartFrame;
            currentEndFrame = idleEndFrame;
            PlayFrames();
        }

        public void PlayWalkAnimation()
        {
            currentStartFrame = walkStartFrame;
            currentEndFrame = walkEndFrame;
            PlayFrames();
        }

        public void PlayAttackAnimation()
        {
            currentStartFrame = attackStartFrame;
            currentEndFrame = attackEndFrame;
            PlayFrames();
        }

        public void PlayDeathAnimation() 
        {
            currentStartFrame = deathStartFrame;
            currentEndFrame = deathEndFrame;
            PlayFramesNoLoop();
        }

        public void PlayHitAnimation()
        {
            currentStartFrame = hitStartFrame;
            currentEndFrame = hitEndFrame;
            PlayFrames();
        }

        //Plays the frames
        public void PlayFrames ()
        {
            if (Counter())
            {
                if (spriteSheet.CurrentFrame < currentEndFrame)
                {
                    spriteSheet.CurrentFrame += 1;
                }
                else
                {
                    spriteSheet.CurrentFrame = currentStartFrame;
                }
            }
        }

        private bool aux = false;
        private bool animationFinished = false;
        public void PlayFramesNoLoop()
        {
            if (!aux)
            {
                spriteSheet.CurrentFrame = deathStartFrame;
                aux = true;
            }

            if (!animationFinished)
            {
                if (Counter())
                {
                    if (spriteSheet.CurrentFrame < currentEndFrame)
                    {
                        spriteSheet.CurrentFrame += 1;
                    }
                    else
                    {
                        spriteSheet.CurrentFrame = deathEndFrame;
                        animationFinished = true;
                    }
                }
            }
        }

        private bool Counter ()
        {
            if (clock >=  animationSpeed * 0.01)
            {
                clock = 0;
                return true;
            }

            clock += 1 * (float)Game.UpdateTime.Elapsed.TotalSeconds;

            return false;
        }

        private void CheckComponents()
        {
            if (spriteComponent == null || spriteSheet == null)
            {
                DebugText.Print(Entity.Name + " has null components!!", new Int2(500, 300));
            }
        }
    }
}
