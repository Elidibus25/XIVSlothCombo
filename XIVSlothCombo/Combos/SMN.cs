using Dalamud.Game.ClientState.JobGauge.Types;

namespace XIVSlothComboPlugin.Combos
{
    internal static class SMN
    {
        public const byte ClassID = 26;
        public const byte JobID = 27;

        public const float CooldownThreshold = 0.5f;

        public const uint
            // summons
            SummonRuby = 25802,
            SummonTopaz = 25803,
            SummonEmerald = 25804,

            SummonIfrit = 25805,
            SummonTitan = 25806,
            SummonGaruda = 25807,

            SummonIfrit2 = 25838,
            SummonTitan2 = 25839,
            SummonGaruda2 = 25840,

            SummonCarbuncle = 25798,

            // summon abilities
            Gemshine = 25883,
            PreciousBrilliance = 25884,
            DreadwyrmTrance = 3581,

            // summon ruins
            RubyRuin1 = 25808,
            RubyRuin2 = 25811,
            RubyRuin3 = 25817,
            TopazRuin1 = 25809,
            TopazRuin2 = 25812,
            TopazRuin3 = 25818,
            EmeralRuin1 = 25810,
            EmeralRuin2 = 25813,
            EmeralRuin3 = 25819,

            // summon outbursts
            Outburst = 16511,
            RubyOutburst = 25814,
            TopazOutburst = 25815,
            EmeraldOutburst = 25816,

            // summon single targets
            RubyRite = 25823,
            TopazRite = 25824,
            EmeraldRite = 25825,

            // summon aoes
            RubyCata = 25832,
            TopazCata = 25833,
            EmeraldCata = 25834,

            // summon astral flows
            CrimsonCyclone = 25835, // dash
            CrimsonStrike = 25885, // melee
            MountainBuster = 25836,
            Slipstream = 25837,

            // demisummons
            SummonBahamut = 7427,
            SummonPhoenix = 25831,

            // demisummon abilities
            AstralImpulse = 25820, // single target bahamut gcd
            AstralFlare = 25821, // aoe bahamut gcd
            Deathflare = 3582, // damage ogcd bahamut
            EnkindleBahamut = 7429,

            FountainOfFire = 16514, // single target phoenix gcd
            BrandOfPurgatory = 16515, // aoe phoenix gcd
            Rekindle = 25830, // healing ogcd phoenix
            EnkindlePhoenix = 16516,

            // shared summon abilities
            AstralFlow = 25822,

            // summoner gcds
            Ruin = 163,
            Ruin2 = 172,
            Ruin3 = 3579,
            Ruin4 = 7426,
            Tridisaster = 25826,

            // summoner AoE
            RubyDisaster = 25827,
            TopazDisaster = 25828,
            EmeraldDisaster = 25829,

            // summoner ogcds
            EnergyDrain = 16508,
            Fester = 181,
            EnergySiphon = 16510,
            Painflare = 3578,

            // revive
            Resurrection = 173,

            // buff 
            RadiantAegis = 25799,
            Aethercharge = 25800,
            SearingLight = 25801;


        public static class Buffs
        {
            public const ushort
                FurtherRuin = 2701,
                GarudasFavor = 2725,
                TitansFavor = 2853,
                IfritsFavor = 2724,
                EverlastingFlight = 16517,
                SearingLight = 2703;
        }

        public static class Levels
        {
            public const byte
                Aethercharge = 6,
                SummonRuby = 6,
                SummonTopaz = 15,
                SummonEmerald = 22,
                Painflare = 52,
                Ruin3 = 54,
                AstralFlow = 60,
                EnhancedEgiAssault = 74,
                Ruin4 = 62,
                EnergyDrain = 10,
                EnergySiphon = 52,
                OutburstMastery2 = 82,
                Slipstream = 86,
                MountainBuster = 86,
                SearingLight = 66,

                Bahamut = 70,
                Phoenix = 80,

                // summoner ruins lvls
                RubyRuin1 = 22,
                RubyRuin2 = 30,
                RubyRuin3 = 54,
                TopazRuin1 = 22,
                TopazRuin2 = 30,
                TopazRuin3 = 54,
                EmeralRuin1 = 22,
                EmeralRuin2 = 30,
                EmeralRuin3 = 54;
        }

        public static class Config
        {
            public const string
                SMNLucidDreamingFeature = "SMNLucidDreamingFeature",
                SMNSearingLightChoice = "SMNSearingLightChoice",
                SummonerBurstPhase = "SummonerBurstPhase",
                SummonerPrimalChoice = "SummonerPrimalChoice";
        }
    }
    internal class SummonerRaiseFeature : CustomCombo
    {
        protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.SummonerRaiseFeature;

        protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
        {
            if (actionID == All.Swiftcast)
            {
                if (IsOnCooldown(All.Swiftcast))
                    return SMN.Resurrection;
            }
            return actionID;
        }
    }

    internal class SummonerSpecialRuinFeature : CustomCombo
    {
        protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.SummonerSpecialRuinFeature;

        protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
        {
            if (actionID == SMN.Ruin4)
            {
                var furtherRuin = HasEffect(SMN.Buffs.FurtherRuin);
                if (!furtherRuin)
                    return SMN.Ruin3;
            }
            return actionID;
        }
    }

    internal class SummonerEDFesterCombo : CustomCombo
    {
        protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.SummonerEDFesterCombo;

        protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
        {
            if (actionID == SMN.Fester)
            {
                var gauge = GetJobGauge<SMNGauge>();
                if (HasEffect(SMN.Buffs.FurtherRuin) && IsOnCooldown(SMN.EnergyDrain) && !gauge.HasAetherflowStacks && IsEnabled(CustomComboPreset.SummonerFesterPainflareRuinFeature))
                    return SMN.Ruin4;
                if (level >= SMN.Levels.EnergyDrain && !gauge.HasAetherflowStacks)
                    return SMN.EnergyDrain;
            }

            return actionID;
        }
    }

    internal class SummonerESPainflareCombo : CustomCombo
    {
        protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.SummonerESPainflareCombo;

        protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
        {
            if (actionID == SMN.Painflare)
            {
                var gauge = GetJobGauge<SMNGauge>();
                if (HasEffect(SMN.Buffs.FurtherRuin) && IsOnCooldown(SMN.EnergySiphon) && !gauge.HasAetherflowStacks && IsEnabled(CustomComboPreset.SummonerFesterPainflareRuinFeature))
                    return SMN.Ruin4;
                if (level >= SMN.Levels.EnergySiphon && !gauge.HasAetherflowStacks)
                    return SMN.EnergySiphon;

            }

            return actionID;
        }
    }

    internal class SummonerMainComboFeature : CustomCombo
    {
        protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.SummonerMainComboFeature;

        protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
        {
            var gauge = GetJobGauge<SMNGauge>();
            var summonerPrimalChoice = Service.Configuration.GetCustomIntValue(SMN.Config.SummonerPrimalChoice);
            var SummonerBurstPhase = Service.Configuration.GetCustomIntValue(SMN.Config.SummonerBurstPhase);
            var lucidThreshold = Service.Configuration.GetCustomIntValue(SMN.Config.SMNLucidDreamingFeature);

            if (actionID is SMN.Ruin or SMN.Ruin2 && InCombat())
            {
                if (CanSpellWeave(actionID))
                {
                    // Searing Light
                    if (IsEnabled(CustomComboPreset.SearingLightonRuinFeature) && IsOffCooldown(SMN.SearingLight) && level >= SMN.Levels.SearingLight)
                    {
                        if (IsEnabled(CustomComboPreset.SummonerSearingLightBurstOption))
                        {
                            if ((SummonerBurstPhase == 1 && OriginalHook(SMN.Ruin) == SMN.AstralImpulse) ||
                                (SummonerBurstPhase == 2 && OriginalHook(SMN.Ruin) == SMN.FountainOfFire))
                                return SMN.SearingLight;
                        }

                        else return SMN.SearingLight;
                    }

                    // ED & Fester
                    if (IsEnabled(CustomComboPreset.SummonerEDMainComboFeature))
                    {
                        if (gauge.HasAetherflowStacks)
                        {
                            if (IsNotEnabled(CustomComboPreset.SummonerEDPoolonMainFeature))
                                return SMN.Fester;
                            if (IsEnabled(CustomComboPreset.SummonerEDPoolonMainFeature))
                            {
                                if (level < SMN.Levels.SearingLight)
                                    return SMN.Fester;
                                if (HasEffect(SMN.Buffs.SearingLight) && 
                                    ((SummonerBurstPhase == 1 && OriginalHook(SMN.Ruin) == SMN.AstralImpulse) ||
                                    (SummonerBurstPhase == 2 && OriginalHook(SMN.Ruin) == SMN.FountainOfFire)))
                                    return SMN.Fester;
                            }
                        }

                        if (level >= SMN.Levels.EnergyDrain && !gauge.HasAetherflowStacks && IsOffCooldown(SMN.EnergyDrain))
                            return SMN.EnergyDrain;
                    }

                    // Lucid
                    if (IsEnabled(CustomComboPreset.SMNLucidDreamingFeature) && IsOffCooldown(All.LucidDreaming) && LocalPlayer.CurrentMp <= lucidThreshold && level >= All.Levels.LucidDreaming)
                        return All.LucidDreaming;
                }

                // Egi Features
                if (IsEnabled(CustomComboPreset.EgisOnRuinFeature))
                {
                    //Swiftcast Garuda Feature                                 
                    if (IsEnabled(CustomComboPreset.SummonerSwiftcastFeatureGaruda) && level >= SMN.Levels.Slipstream)
                    {
                        if (CanSpellWeave(actionID) && IsOffCooldown(All.Swiftcast) && HasEffect(SMN.Buffs.GarudasFavor) && gauge.IsGarudaAttuned)
                            return All.Swiftcast;
                        if (IsEnabled(CustomComboPreset.SummonerGarudaUniqueFeature) && gauge.IsGarudaAttuned && HasEffect(SMN.Buffs.GarudasFavor) && HasEffect(All.Buffs.Swiftcast))
                            return OriginalHook(SMN.AstralFlow);
                    }

                    //Swiftcast Ifrit Feature (Conditions to allow for SpS Ruins to still be under the effect of Swiftcast)
                    if (IsEnabled(CustomComboPreset.SummonerSwiftcastFeatureIfrit) && level >= SMN.Levels.RubyRuin1 && CanSpellWeave(actionID))
                    {
                        if (IsOffCooldown(All.Swiftcast) && level >= All.Levels.Swiftcast && gauge.IsIfritAttuned)
                        {
                            if (IsNotEnabled(CustomComboPreset.SummonerIfritUniqueFeature) ||
                                (IsEnabled(CustomComboPreset.SummonerIfritUniqueFeature) && lastComboMove is SMN.CrimsonStrike or SMN.RubyRuin1 or SMN.RubyRuin2 or SMN.RubyRuin3 or SMN.RubyRite))
                                return All.Swiftcast;
                        }
                    }

                    if (IsEnabled(CustomComboPreset.SummonerGarudaUniqueFeature) && IsNotEnabled(CustomComboPreset.SummonerSwiftcastFeatureGaruda) && gauge.IsGarudaAttuned && HasEffect(SMN.Buffs.GarudasFavor) || //Garuda
                        IsEnabled(CustomComboPreset.SummonerTitanUniqueFeature) && HasEffect(SMN.Buffs.TitansFavor) && lastComboMove == SMN.TopazRite && CanSpellWeave(actionID) || //Titan
                        IsEnabled(CustomComboPreset.SummonerIfritUniqueFeature) && (gauge.IsIfritAttuned && HasEffect(SMN.Buffs.IfritsFavor) || gauge.IsIfritAttuned && lastComboMove == SMN.CrimsonCyclone)) //Ifrit
                        return OriginalHook(SMN.AstralFlow);

                    if (IsEnabled(CustomComboPreset.SummonerEgiAttacksFeature) && (gauge.IsGarudaAttuned || gauge.IsTitanAttuned || gauge.IsIfritAttuned))
                        return OriginalHook(SMN.Gemshine);

                    if (IsEnabled(CustomComboPreset.SummonerEgiSummonsonMainFeature) && gauge.SummonTimerRemaining == 0 && IsOnCooldown(SMN.SummonPhoenix) && IsOnCooldown(SMN.SummonBahamut))
                    {
                        if (gauge.IsIfritReady && !gauge.IsTitanReady && !gauge.IsGarudaReady && level >= SMN.Levels.SummonRuby)
                            return OriginalHook(SMN.SummonRuby);

                        if (summonerPrimalChoice == 1)
                        {
                            if (gauge.IsTitanReady && level >= SMN.Levels.SummonTopaz)
                                return OriginalHook(SMN.SummonTopaz);

                            if (gauge.IsGarudaReady && level >= SMN.Levels.SummonEmerald)
                                return OriginalHook(SMN.SummonEmerald);
                        }

                        if (summonerPrimalChoice == 2)
                        {
                            if (gauge.IsGarudaReady && level >= SMN.Levels.SummonEmerald)
                                return OriginalHook(SMN.SummonEmerald);

                            if (gauge.IsTitanReady && level >= SMN.Levels.SummonTopaz)
                                return OriginalHook(SMN.SummonTopaz);
                        }
                    }
                }

                //Demi Features
                if (IsEnabled(CustomComboPreset.SummonerDemiSummonsFeature))
                {
                    if (gauge.AttunmentTimerRemaining == 0 && gauge.SummonTimerRemaining == 0 && IsOffCooldown(OriginalHook(SMN.Aethercharge)) &&
                        (level >= SMN.Levels.Aethercharge && level < SMN.Levels.Bahamut || //Pre Bahamut Phase
                        gauge.IsBahamutReady && level >= SMN.Levels.Bahamut || //Bahamut Phase
                        gauge.IsPhoenixReady && level >= SMN.Levels.Phoenix)) //Phoenix Phase
                        return OriginalHook(SMN.Aethercharge);

                    if (IsEnabled(CustomComboPreset.SummonerSingleTargetDemiFeature) && CanSpellWeave(actionID))
                    {
                        if (IsOffCooldown(OriginalHook(SMN.AstralFlow)) && level >= SMN.Levels.AstralFlow && (level < SMN.Levels.Bahamut || lastComboMove is SMN.AstralImpulse))
                            return OriginalHook(SMN.AstralFlow);

                        if (IsOffCooldown(OriginalHook(SMN.EnkindleBahamut)) && level >= SMN.Levels.Bahamut && lastComboMove is SMN.AstralImpulse or SMN.FountainOfFire)
                            return OriginalHook(SMN.EnkindleBahamut);
                    }

                    if (IsEnabled(CustomComboPreset.SummonerSingleTargetRekindleOption))
                    {
                        if (IsOffCooldown(OriginalHook(SMN.AstralFlow)) && lastComboMove is SMN.FountainOfFire)
                            return OriginalHook(SMN.AstralFlow);
                    }
                }

                if (IsEnabled(CustomComboPreset.SummonerRuin4ToRuin3Feature) && level >= SMN.Levels.Ruin4 && gauge.SummonTimerRemaining == 0 && gauge.AttunmentTimerRemaining == 0 && HasEffect(SMN.Buffs.FurtherRuin))
                    return SMN.Ruin4;
            }

            return actionID;
        }
    }

    internal class SummonerAOEComboFeature : CustomCombo
    {
        protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.SummonerAOEComboFeature;

        protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
        {
            var gauge = GetJobGauge<SMNGauge>();
            var lucidThreshold = Service.Configuration.GetCustomIntValue(SMN.Config.SMNLucidDreamingFeature);

            if (actionID is SMN.Tridisaster or SMN.Outburst)
            {
                if (InCombat())
                {
                    if (CanSpellWeave(actionID))
                    {
                        var searingChoice = Service.Configuration.GetCustomIntValue(SMN.Config.SMNSearingLightChoice);

                        // Searing
                        if (IsEnabled(CustomComboPreset.BuffOnSimpleAoESummoner) &&
                            IsOffCooldown(SMN.SearingLight) &&
                            level >= SMN.Levels.SearingLight &&
                            (searingChoice == 0 ||
                            (OriginalHook(SMN.Tridisaster) is SMN.AstralFlare && gauge.SummonTimerRemaining > 0 && searingChoice == 1) ||
                            (OriginalHook(SMN.Tridisaster) is SMN.BrandOfPurgatory && gauge.SummonTimerRemaining > 0 && searingChoice == 2) ||
                            (OriginalHook(SMN.PreciousBrilliance) is (SMN.RubyCata or SMN.RubyOutburst) && gauge.SummonTimerRemaining > 0 && searingChoice == 3) ||
                            (OriginalHook(SMN.PreciousBrilliance) is (SMN.EmeraldCata or SMN.EmeraldOutburst) && gauge.SummonTimerRemaining > 0 && searingChoice == 4) ||
                            (OriginalHook(SMN.PreciousBrilliance) is (SMN.TopazCata or SMN.TopazOutburst) && gauge.SummonTimerRemaining > 0 && searingChoice == 5)))
                            return SMN.SearingLight;


                        // ED & Fester
                        if (IsEnabled(CustomComboPreset.SummonerESAOEFeature))
                        {
                            if (gauge.HasAetherflowStacks && HasEffect(SMN.Buffs.SearingLight))
                                return SMN.Painflare;
                            if (level >= SMN.Levels.EnergySiphon && !gauge.HasAetherflowStacks && IsOffCooldown(SMN.EnergySiphon))
                                return SMN.EnergySiphon;
                        }

                        // Lucid
                        if (IsEnabled(CustomComboPreset.SMNLucidDreamingFeature) && IsOffCooldown(All.LucidDreaming) && LocalPlayer.CurrentMp <= lucidThreshold && level >= All.Levels.LucidDreaming)
                            return All.LucidDreaming;

                        //Demi Nuke
                        if (IsEnabled(CustomComboPreset.SummonerAOEDemiFeature) && CanSpellWeave(actionID))
                        {
                            if (IsOffCooldown(OriginalHook(SMN.AstralFlow)) &&
                                level >= SMN.Levels.AstralFlow &&
                                (level < SMN.Levels.Bahamut || lastComboMove is SMN.AstralFlare) &&
                                gauge.AttunmentTimerRemaining > 0)
                                return OriginalHook(SMN.AstralFlow);

                            if (IsOffCooldown(OriginalHook(SMN.EnkindleBahamut)) &&
                                level >= SMN.Levels.Bahamut &&
                                OriginalHook(SMN.Tridisaster) is SMN.AstralFlare or SMN.BrandOfPurgatory &&
                                gauge.SummonTimerRemaining > 0)
                                return OriginalHook(SMN.EnkindleBahamut);
                        }

                        //Demi Nuke 2: Electric Boogaloo
                        if (IsEnabled(CustomComboPreset.SummonerAOETargetRekindleOption))
                        {
                            if (IsOffCooldown(OriginalHook(SMN.AstralFlow)) &&
                                OriginalHook(SMN.Tridisaster) is SMN.BrandOfPurgatory)
                                return OriginalHook(SMN.AstralFlow);
                        }
                    }

                   

                    //Demi
                    if (IsEnabled(CustomComboPreset.SummonerDemiAoESummonsFeature))
                    {
                        if (gauge.AttunmentTimerRemaining == 0 && gauge.SummonTimerRemaining == 0 && IsOffCooldown(OriginalHook(SMN.Aethercharge)) &&
                            (level >= SMN.Levels.Aethercharge && level < SMN.Levels.Bahamut || //Pre Bahamut Phase
                            gauge.IsBahamutReady && level >= SMN.Levels.Bahamut || //Bahamut Phase
                            gauge.IsPhoenixReady && level >= SMN.Levels.Phoenix) && //Phoenix Phase
                            !gauge.IsIfritReady && 
                            !gauge.IsTitanReady && 
                            !gauge.IsGarudaReady) 
                            return OriginalHook(SMN.Aethercharge);

                    }


                    // Egis
                    if (IsEnabled(CustomComboPreset.EgisOnAOEFeature))
                    {
                        if (IsEnabled(CustomComboPreset.SummonerGarudaUniqueFeature) && gauge.IsGarudaAttuned && HasEffect(SMN.Buffs.GarudasFavor) || //Garuda
                            IsEnabled(CustomComboPreset.SummonerTitanUniqueFeature) && HasEffect(SMN.Buffs.TitansFavor) && lastComboMove == SMN.TopazCata && CanSpellWeave(actionID) || //Titan
                            IsEnabled(CustomComboPreset.SummonerIfritUniqueFeature) && (gauge.IsIfritAttuned && HasEffect(SMN.Buffs.IfritsFavor) || gauge.IsIfritAttuned && lastComboMove == SMN.CrimsonCyclone)) //Ifrit
                            return OriginalHook(SMN.AstralFlow);

                        if (gauge.AttunmentTimerRemaining == 0 && gauge.SummonTimerRemaining == 0)
                        {
                            if (gauge.IsTitanReady && level >= SMN.Levels.SummonTopaz)
                                return OriginalHook(SMN.SummonTopaz);
                            if (gauge.IsGarudaReady && level >= SMN.Levels.SummonEmerald)
                                return OriginalHook(SMN.SummonEmerald);
                            if (gauge.IsIfritReady && level >= SMN.Levels.SummonRuby)
                                return OriginalHook(SMN.SummonRuby);
                        }
                    }

                    //Precious Brilliance
                    if (IsEnabled(CustomComboPreset.SummonerEgiAttacksAOEFeature) && (gauge.IsGarudaAttuned || gauge.IsTitanAttuned || gauge.IsIfritAttuned))
                        return OriginalHook(SMN.PreciousBrilliance);



                    if (IsEnabled(CustomComboPreset.SummonerRuin4ToTridisasterFeature) && level >= SMN.Levels.Ruin4 && gauge.SummonTimerRemaining == 0 && gauge.AttunmentTimerRemaining == 0 && HasEffect(SMN.Buffs.FurtherRuin))
                        return SMN.Ruin4;
                }
            }

            return actionID;
        }
    }

    internal class SummonerCarbuncleSummonFeature : CustomCombo
    {
        protected internal override CustomComboPreset Preset => CustomComboPreset.SummonerCarbuncleSummonFeature;

        protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
        {
            var gauge = GetJobGauge<SMNGauge>();
            if (actionID is SMN.Ruin or SMN.Ruin2 or SMN.Ruin3 or SMN.DreadwyrmTrance or SMN.AstralFlow or SMN.EnkindleBahamut or SMN.SearingLight or SMN.RadiantAegis or SMN.Outburst or SMN.Tridisaster or SMN.PreciousBrilliance or SMN.Gemshine)
            {
                if (!HasPetPresent() && gauge.SummonTimerRemaining == 0 && gauge.Attunement == 0)
                    return SMN.SummonCarbuncle;
            }

            return actionID;
        }
    }

    internal class SummonerAstralFlowonSummonsFeature : CustomCombo
    {
        protected internal override CustomComboPreset Preset => CustomComboPreset.SummonerAstralFlowonSummonsFeature;

        protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
        {
            var gauge = GetJobGauge<SMNGauge>();
            if (actionID is SMN.SummonEmerald or SMN.SummonRuby or SMN.SummonTopaz or SMN.SummonIfrit or SMN.SummonTitan or SMN.SummonGaruda or SMN.SummonIfrit2 or SMN.SummonTitan2 or SMN.SummonGaruda2)
            {
                if (HasEffect(SMN.Buffs.TitansFavor) || HasEffect(SMN.Buffs.GarudasFavor) || HasEffect(SMN.Buffs.IfritsFavor) || lastComboMove == SMN.CrimsonCyclone)
                    return OriginalHook(SMN.AstralFlow);
            }

            return actionID;
        }
    }
}
