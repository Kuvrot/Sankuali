﻿using Stride.Engine;
using Stride.Core;
using Stride.Rendering.Sprites;
using Stride.Audio;

namespace Furia.Stats
{
    public class WeaponStats : SyncScript
    {
        //Weapon properties
        public byte weaponID = 0;
        public float MaxShootDistance = 100f;
        public float ShootImpulse = 5f;
        public float Cooldown = 0.3f;
        public float ReloadCooldown = 2.0f;
        public SpriteComponent RemainingBullets;
        public int remainingAmmo = 0;
        public int maxBullets = 30;
        public int inventoryAmmo = 100;
        public bool infiniteAmmo = false;
        public bool isMelee = false;
        public float damage = 50;

        // Sound
        // If any or them are null, then the sound will not be reproduced.
        public Sound shotSound;
        public Sound reloadSound;

        //WeaponAnimations (optional)
        [Display("Spritesheet Animations (optional)")]
        public bool isSpriteSheetAnimation = true; // Controls if the weapon will use sprite animations
        public byte animationSpeed = 12; //Idk how this is working, but lesser is faster
        public byte reloadStartFrame = 0, reloadEndFrame = 0;
        public byte shootStartFrame = 0, shootEndFrame = 0;

        private byte currentStartFrame = 0, currentEndFrame = 0;
        private float clock = 0;

        private bool playingAnimation = false;

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
            if (playingAnimation)
            {
                if (Counter())
                {
                    if (spriteSheet.CurrentFrame < currentEndFrame)
                    {
                        spriteSheet.CurrentFrame += 1;
                    }
                    else
                    {
                        spriteSheet.CurrentFrame = 0;
                        playingAnimation = false;
                    }
                }
            }
        }
        public void PlayReloadAnimation()
        {
            spriteSheet.CurrentFrame = reloadStartFrame;
            currentStartFrame = reloadStartFrame;
            currentEndFrame = reloadEndFrame;
            playingAnimation = true;
        }

        public void PlayShootAnimation()
        {
            spriteSheet.CurrentFrame = shootStartFrame;
            currentStartFrame = shootStartFrame;
            currentEndFrame = shootEndFrame;
            playingAnimation = true;
        }
        private bool Counter()
        {
            if (clock >= animationSpeed * 0.01)
            {
                clock = 0;
                return true;
            }

            clock += 1 * (float)Game.UpdateTime.Elapsed.TotalSeconds;

            return false;
        }
    }
}
