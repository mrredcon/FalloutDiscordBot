using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.Addons.Preconditions;
using DotNetDiscordBot.Services;

namespace DotNetDiscordBot.Modules
{
    public class RngModule : ModuleBase<SocketCommandContext>
    {
        [Group("roll")]
        public class RollModule : ModuleBase<SocketCommandContext>
        {
            public RollService RollService { get; set; }

            [Command("barter"), Ratelimit(1, 5, Measure.Seconds)]
            public async Task RollBarter() { await RollSkill(Character.SkillEnum.barter); }
            [Command("explosives"), Ratelimit(1, 5, Measure.Seconds)]
            public async Task RollExplosives() { await RollSkill(Character.SkillEnum.explosives); }
            [Command("guns"), Ratelimit(1, 5, Measure.Seconds)]
            public async Task RollGuns() { await RollSkill(Character.SkillEnum.guns); }
            [Command("lockpick"), Ratelimit(1, 5, Measure.Seconds)]
            public async Task RollLockpick() { await RollSkill(Character.SkillEnum.lockpick); }
            [Command("medicine"), Ratelimit(1, 5, Measure.Seconds)]
            public async Task RollMedicine() { await RollSkill(Character.SkillEnum.medicine); }
            [Command("melee"), Ratelimit(1, 5, Measure.Seconds)]
            public async Task RollMeleeWeapons() { await RollSkill(Character.SkillEnum.meleeweapons); }
            [Command("repair"), Ratelimit(1, 5, Measure.Seconds)]
            public async Task RollRepair() { await RollSkill(Character.SkillEnum.repair); }
            [Command("science"), Ratelimit(1, 5, Measure.Seconds)]
            public async Task RollScience() { await RollSkill(Character.SkillEnum.science); }
            [Command("sneak"), Ratelimit(1, 5, Measure.Seconds)]
            public async Task RollSneak() { await RollSkill(Character.SkillEnum.sneak); }
            [Command("speech"), Ratelimit(1, 5, Measure.Seconds)]
            public async Task RollSpeech() { await RollSkill(Character.SkillEnum.speech); }
            [Command("survival"), Ratelimit(1, 5, Measure.Seconds)]
            public async Task RollSurvival() { await RollSkill(Character.SkillEnum.survival); }
            [Command("unarmed"), Ratelimit(1, 5, Measure.Seconds)]
            public async Task RollUnarmed() { await RollSkill(Character.SkillEnum.unarmed); }

            public async Task RollSkill(Character.SkillEnum skill)
            {
                await ReplyAsync(RollService.GetRollResult(skill, Context.User));
            }

            [Command("strength"), Ratelimit(1,5,Measure.Seconds)]
            public async Task RollStrength() { await RollSpecial(Character.SpecialEnum.Strength); }
            [Command("perception"), Ratelimit(1, 5, Measure.Seconds)]
            public async Task RollPerception() { await RollSpecial(Character.SpecialEnum.Perception); }
            [Command("endurance"), Ratelimit(1, 5, Measure.Seconds)]
            public async Task RollEndurance() { await RollSpecial(Character.SpecialEnum.Endurance); }
            [Command("charisma"), Ratelimit(1, 5, Measure.Seconds)]
            public async Task RolLCharisma() { await RollSpecial(Character.SpecialEnum.Charisma); }
            [Command("intelligence"), Ratelimit(1, 5, Measure.Seconds)]
            public async Task RollIntelligence() { await RollSpecial(Character.SpecialEnum.Intelligence); }
            [Command("agility"), Ratelimit(1, 5, Measure.Seconds)]
            public async Task RollAgility() { await RollSpecial(Character.SpecialEnum.Agility); }
            [Command("luck"), Ratelimit(1, 5, Measure.Seconds)]
            public async Task RollLuck() { await RollSpecial(Character.SpecialEnum.Luck); }
            public async Task RollSpecial(Character.SpecialEnum special)
            {
                await ReplyAsync(RollService.GetRollResult(special, Context.User));
            }
        }
    }
}
