using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Regressus.NPCs;
using ReLogic.Content;

namespace Regressus.NPCs.Starshroom
{
    public class Starlad : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 13;
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (Main.player[NPC.target].ZoneOverworldHeight && !Main.dayTime)
                return 0.35f;
            return 0;
        }
        public override void SetDefaults()
        {
            NPC.height = 56;
            NPC.width = 68;
            NPC.damage = 0;
            NPC.friendly = false;
            NPC.lifeMax = 75;
            NPC.defense = 2;
            NPC.aiStyle = 0;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
        }
        public override bool PreDraw(SpriteBatch sb, Vector2 screenPos, Color drawColor)
        {
            SpriteEffects effects = NPC.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            Vector2 origin = new Vector2(ModContent.Request<Texture2D>("Regressus/NPCs/Starshroom/Starlad").Value.Width / 2, NPC.height / 2);
            sb.Draw(ModContent.Request<Texture2D>("Regressus/NPCs/Starshroom/Starlad").Value, NPC.Center - screenPos, NPC.frame, drawColor, NPC.rotation, origin, NPC.scale, effects, 0f);
            sb.Draw(ModContent.Request<Texture2D>("Regressus/NPCs/Starshroom/Starlad_Glow").Value, NPC.Center - screenPos, NPC.frame, Color.White, NPC.rotation, origin, NPC.scale, effects, 0f);
            if (AIState != Idle)
            {
                sb.Draw(ModContent.Request<Texture2D>("Regressus/NPCs/Starshroom/Starlad_star").Value, NPC.Center - screenPos, NPC.frame, Color.White * alpha, NPC.rotation, origin, NPC.scale, effects, 0f);
            }
            return false;
        }
        public override void FindFrame(int frameHeight)
        {
            if (AIState == Idle)
            {
                NPC.frameCounter++;
                if (NPC.frameCounter < 5)
                {
                    NPC.frame.Y = 8 * frameHeight;
                }
                else if (NPC.frameCounter < 10)
                {
                    NPC.frame.Y = 9 * frameHeight;
                }
                else if (NPC.frameCounter < 15)
                {
                    NPC.frame.Y = 10 * frameHeight;
                }
                else if (NPC.frameCounter < 20)
                {
                    NPC.frame.Y = 11 * frameHeight;
                }
                else if (NPC.frameCounter < 25)
                {
                    NPC.frame.Y = 12 * frameHeight;
                }
                else
                {
                    NPC.frameCounter = 0;
                }
            }
            else if (AIState == Angery)
            {
                NPC.frame.Y = 0;
            }
            else if (AIState == Attack)
            {
                NPC.frameCounter++;
                if (NPC.frameCounter < 5)
                {
                    NPC.frame.Y = 0 * frameHeight;
                }
                else if (NPC.frameCounter < 10)
                {
                    NPC.frame.Y = 1 * frameHeight;
                }
                else if (NPC.frameCounter < 15)
                {
                    NPC.frame.Y = 2 * frameHeight;
                }
                else if (NPC.frameCounter < 20)
                {
                    NPC.frame.Y = 3 * frameHeight;
                }
                else if (NPC.frameCounter < 25)
                {
                    NPC.frame.Y = 4 * frameHeight;
                }
                else if (NPC.frameCounter < 30)
                {
                    NPC.frame.Y = 5 * frameHeight;
                }
                else if (NPC.frameCounter < 35)
                {
                    NPC.frame.Y = 6 * frameHeight;
                }
                else if (NPC.frameCounter < 40)
                {
                    NPC.frame.Y = 7 * frameHeight;
                }
                else
                {
                    NPC.frameCounter = 0;
                }
            }
        }
        private const int AISlot = 0;
        private const int TimerSlot = 1;
        private const int Idle = 0;
        private const int Angery = 1;
        private const int Attack = 2;
        public float AIState
        {
            get => NPC.ai[AISlot];
            set => NPC.ai[AISlot] = value;
        }

        public float AITimer
        {
            get => NPC.ai[TimerSlot];
            set => NPC.ai[TimerSlot] = value;
        }
        bool hasAggrod;
        float alpha;
        public override bool CheckDead()
        {
            Color newColor7 = Color.CornflowerBlue;
            for (int num613 = 0; num613 < 7; num613++)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, 58, NPC.velocity.X * 0.1f, NPC.velocity.Y * 0.1f, 150, default(Color), 0.8f);
            }
            for (float num614 = 0f; num614 < 1f; num614 += 0.125f)
            {
                Dust.NewDustPerfect(NPC.Center, 278, Vector2.UnitY.RotatedBy(num614 * ((float)Math.PI * 2f) + Main.rand.NextFloat() * 0.5f) * (4f + Main.rand.NextFloat() * 4f), 150, newColor7).noGravity = true;
            }
            for (float num615 = 0f; num615 < 1f; num615 += 0.25f)
            {
                Dust.NewDustPerfect(NPC.Center, 278, Vector2.UnitY.RotatedBy(num615 * ((float)Math.PI * 2f) + Main.rand.NextFloat() * 0.5f) * (2f + Main.rand.NextFloat() * 3f), 150, Color.Gold).noGravity = true;
            }
            Vector2 vector52 = new Vector2(Main.screenWidth, Main.screenHeight);
            if (NPC.Hitbox.Intersects(Utils.CenteredRectangle(Main.screenPosition + vector52 / 2f, vector52 + new Vector2(400f))))
            {
                for (int num616 = 0; num616 < 7; num616++)
                {
                    Gore.NewGore(NPC.position, Main.rand.NextVector2CircularEdge(0.5f, 0.5f) * NPC.velocity.Length(), Utils.SelectRandom<int>(Main.rand, 16, 17, 17, 17, 17, 17, 17, 17));
                }
            }
            return true;
        }
        public override void AI()
        {
            Player player = Main.player[NPC.target];
            NPC.TargetClosest(true);
            if (NPC.life != NPC.lifeMax && !hasAggrod)
            {
                NPC.frameCounter = 0;
                AIState = Angery;
                hasAggrod = true;
            }
            if (AIState == Idle)
            {

            }
            else if (AIState == Angery)
            {
                AITimer++;
                alpha = MathHelper.Lerp(0, 1, AITimer / 100);
                if (AITimer >= 45)
                {
                    AITimer = 0;
                    AIState = Attack;
                    NPC.damage = 15;
                    NPC.knockBackResist = 0f;
                    NPC.noGravity = true;
                    NPC.noTileCollide = true;
                }
            }
            else if (AIState == Attack)
            {
                AITimer++;
                if (AITimer < 60)
                {
                    NPC.Center = Vector2.Lerp(NPC.Center, new Vector2(NPC.Center.X, player.Center.Y), AITimer / 60);
                }
                if (AITimer > 60 && AITimer < 125)
                {
                    for (int num622 = 0; num622 < 4; num622++)
                    {
                        Dust d = Main.dust[Dust.NewDust(NPC.Center, NPC.width / 3, NPC.height / 3, 57, NPC.velocity.X * 0.1f, NPC.velocity.Y * 0.1f, 150, default(Color), 1.2f)];
                        d.noGravity = true;
                    }
                }
                if (AITimer == 60)
                {
                    Terraria.Audio.SoundEngine.PlaySound(SoundID.Item9, NPC.Center);
                    NPC.velocity.X *= 0.98f;
                    Vector2 vector9 = new Vector2(NPC.position.X + (NPC.width * 0.5f), NPC.position.Y + (NPC.height * 0.5f));
                    {
                        float rotation2 = (float)Math.Atan2((vector9.Y) - (player.Center.Y), (vector9.X) - (player.Center.X));
                        NPC.velocity.X = (float)(Math.Cos(rotation2) * 28) * -1;
                    }
                }
                if (AITimer == 145)
                {
                    NPC.velocity = Vector2.Zero;
                }
                if (AITimer >= 185)
                    AITimer = 0;
            }
        }
    }
}
