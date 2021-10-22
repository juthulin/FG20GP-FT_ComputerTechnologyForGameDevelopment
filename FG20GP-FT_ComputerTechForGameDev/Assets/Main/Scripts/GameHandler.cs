using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;

namespace Main.Scripts
{
    public class GameHandler : MonoBehaviour
    {
        [SerializeField] public float timeBetweenShots = .2f;
        [SerializeField] public int enemiesToSpawn = 5;

        private readonly List<GameHandlerComponent> _handlers = new List<GameHandlerComponent>();

        private bool _hasPlayerEntity = false;
        private Entity _player;

        private void Start()
        {
            _handlers.Add(new PlayerHandler(this));
            _handlers.Add(new EnemyHandler(this));
            _handlers.Add(new ProjectileHandler(this));

            for (int i = 0; i < _handlers.Count; i++)
            {
                _handlers[i].Init();
            }
        }

        private void Update()
        {
            for (int i = 0; i < _handlers.Count; i++)
            {
                _handlers[i].Tick();
            }
        }

        public bool GetPlayerPosition(ref float3 playerPosition)
        {
            if (_hasPlayerEntity == false) return false;
            
            EntityManager entManager = World.DefaultGameObjectInjectionWorld.EntityManager;
            playerPosition = entManager.GetComponentData<Translation>(_player).Value;
            
            return true;
        }

        public void SetPlayerRef(Entity playerEntity)
        {
            _player = playerEntity;
            _hasPlayerEntity = true;
        }
    }
}