﻿using System;
using System.Timers;
using CollaborationEngine.Objects;
using CollaborationEngine.Objects.Components;
using CollaborationEngine.Scenes;
using CollaborationEngine.States.Server;
using CollaborationEngine.Tasks;

namespace CollaborationEngine.States
{
    public class ServerCollaborationState : IApplicationState
    {
        public event InputColliderComponent<InstructionObject>.InputEvent OnIndicationObjectClicked;

        public void Initialize()
        {
            _currentState = new SelectTaskState(this);
            _currentState.Initialize();

            ObjectLocator.Instance.ServerRoot.SetActive(true);
            ObjectLocator.Instance.ClientRoot.SetActive(false);

            Scene = new Scene(ObjectLocator.Instance.SceneRoot);
            Scene.OnIndicationObjectAdded += Scene_OnIndicationObjectAdded;

            _synchronizationTimer.Interval = 200.0;
            _synchronizationTimer.Elapsed += SynchronizationTimer_Elapsed;
            _synchronizationTimer.Start();
        }
        public void Shutdown()
        {
            _synchronizationTimer.Stop();
            _synchronizationTimer.Elapsed -= SynchronizationTimer_Elapsed;

            Scene = null;

            if (_currentState != null)
            {
                _currentState.Shutdown();
                _currentState = null;
            }
        }

        public void FixedUpdate()
        {
            _currentState.FixedUpdate();
        }
        public void FrameUpdate()
        {
            _currentState.FrameUpdate();
        }

        public Scene Scene { get; private set; }
        public InstructionType SelectedInstructionType
        {
            get
            {
                return _selectedInstructionType;
            }
            set { _selectedInstructionType = value; }
        }
        public TaskManager TaskManager
        {
            get
            {
                return _taskManager;
            }
        }

        public IApplicationState CurrentState
        {
            get
            {
                return _currentState;
            }
            set
            {
                _currentState.Shutdown();
                _currentState = value;
                _currentState.Initialize();
            }
        }

        private void Scene_OnIndicationObjectAdded(Scene scene, Scene.SceneEventArgs<InstructionObject> eventArgs)
        {
            var sceneObject = eventArgs.SceneObject;

            var inputCollider = new InputColliderComponent<InstructionObject>(sceneObject);
            inputCollider.OnPressed += InputCollider_OnPressed;
        }
        private void InputCollider_OnPressed(InputColliderComponent<InstructionObject> sender, EventArgs eventArgs)
        {
            if (OnIndicationObjectClicked != null)
                OnIndicationObjectClicked(sender, eventArgs);
        }
        private void SynchronizationTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Scene.SynchronizeScene();
        }

        private IApplicationState _currentState;
        private InstructionType _selectedInstructionType = InstructionType.Arrow;
        private readonly Timer _synchronizationTimer = new Timer();
        private readonly TaskManager _taskManager = new TaskManager();
    }
}
