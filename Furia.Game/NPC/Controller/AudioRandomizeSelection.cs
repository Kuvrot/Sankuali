using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stride.Core.Mathematics;
using Stride.Input;
using Stride.Engine;
using Stride.Audio;
using Furia.NPC.Stats;

namespace Furia.NPC.Controller
{
    public class AudioRandomizeSelection : SyncScript
    {
        public List<Sound> HurtSounds = [];

        public override void Start()
        {
            Entity.Get<NpcStats>().hitSound = HurtSounds[new Random().Next(0 , HurtSounds.Count)];
        }

        public override void Update()
        {
            // Do stuff every new frame
        }
    }
}
