﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Vale.Control;
using Vale.GameObjects.Skills;
using Vale.GameObjects.Skills.Hero.Archer;
using Vale.ScreenSystem.Screens;

namespace Vale.GameObjects.Actors
{
    /// <summary>
    ///     The player-controlled Hero
    /// </summary>
    internal class Hero : CombatUnit
    {
        private Skill SkillOne, SkillTwo, SkillThree;
        public MouseProvider MouseProvider { get; private set; }
        public KeyboardProvider KeyboardProvider { get; private set; }

        public Hero(GameplayScreen gameScreen, MouseProvider mouseProvider, KeyboardProvider keyboardProvider, float spawnX = 0, float spawnY = 0, Faction alignment = Faction.Player)
            : this(gameScreen, mouseProvider, keyboardProvider, new Vector2(spawnX, spawnY), alignment) { }

        public Hero(GameplayScreen gameScreen, MouseProvider mouseProvider, KeyboardProvider keyboardProvider, Vector2 spawnPoint, Faction alignment = Faction.Player)
            : base(gameScreen, alignment, spawnPoint)
        {
            MouseProvider = mouseProvider;
            KeyboardProvider = keyboardProvider;
            Speed = 0.3f;

            SkillOne = new QuickShot(gameScreen, this);
            SkillTwo = new SplitShot(gameScreen, this);
            SkillThree = new ReturnShot(gameScreen, this);
            SkillOne.LoadContent();
            SkillTwo.LoadContent();
            SkillThree.LoadContent();
            Game.AddObject(SkillOne);
            Game.AddObject(SkillTwo);
            Game.AddObject(SkillThree);
        }

        public override void Update(GameTime gameTime)
        {
            if (Controllable)
            {
                Velocity = Vector2.Multiply(Input.Instance.NormalizedInput, Speed);
                // always call base
                base.Update(gameTime);

                //make Player handle this. map skills to Commands "XCommand triggers attack1", "BCommand triggers attack 2" etc.
                if (MouseProvider.ButtonPress(MouseProvider.Button.LMB))
                {
                    SkillOne.Execute(MouseProvider.PointerPosition);
                }

                if (MouseProvider.ButtonPress(MouseProvider.Button.RMB))
                {
                    SkillTwo.Execute(MouseProvider.PointerPosition);
                }

                if (KeyboardProvider.KeyPress(' '))
                {
                    SkillThree.Execute(MouseProvider.PointerPosition);
                }

                Rotation = (float)Math.Atan2(
                    MouseProvider.PointerPosition.Y - Position.Y,
                    MouseProvider.PointerPosition.X - Position.X);
            }

            if (KeyboardProvider.KeyPress('P'))
                Console.WriteLine("pX:" + Position.X + " pY:" + Position.Y);
        }
    }
}