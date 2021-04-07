using SharpDX.XInput;
using System;
using System.Threading;
using WindowsInput;

namespace GamepadAsMouse
{
    internal class GamepadAsMouse
    {
        //Dau vao controller
        private Controller _controller;

        //Gia lap chuot
        private IMouseSimulator _mouseSimulator;

        //Trang thai cua phim A
        private bool _wasADown;

        //Trang thai cua phim B
        private bool _wasBDown;

        public GamepadAsMouse(int index)
        {
            getSelectedGamepad(index);

            //Gia lap dau vao la chuot
            _mouseSimulator = new InputSimulator().Mouse;

            Update();
        }

        private void getSelectedGamepad(int index)
        {
            var controllers = new[] { new Controller(UserIndex.One),
                                      new Controller(UserIndex.Two),
                                      new Controller(UserIndex.Three),
                                      new Controller(UserIndex.Four)
            };

            _controller = controllers[index];
        }

        private void Update()
        {
            State previousState;
            _controller.GetState(out previousState);
            while (_controller.IsConnected)
            {
                if (IsKeyPressed(ConsoleKey.Escape))
                {
                    break;
                }
                //Nhan trang thai cua controller
                _controller.GetState(out var state);
                if (!previousState.Equals(state))
                {
                    //Truyen trang thai cua controller vao chuot gia lap
                    Movement(state);
                    Scroll(state);
                    LeftButton(state);
                    RighButtom(state);

                    //In trang thai cua Gamepad
                    Console.WriteLine(state.Gamepad);
                }
                Thread.Sleep(10);
                previousState = state;
            }
        }

        //Mapping hanh dong an chuot phai
        private void RighButtom(State state)
        {
            //Gan phim A la chuot phai
            var isADown = state.Gamepad.Buttons.HasFlag(GamepadButtonFlags.A);
            if (isADown && !_wasADown) _mouseSimulator.LeftButtonDown();
            if (!isADown && _wasADown) _mouseSimulator.LeftButtonUp();
            _wasADown = isADown;
        }

        //Mapping hanh dong nhan chuot trai
        private void LeftButton(State state)
        {
            //Gan phim B la chuot trai
            var isBDown = state.Gamepad.Buttons.HasFlag(GamepadButtonFlags.B);
            if (isBDown && !_wasBDown) _mouseSimulator.RightButtonDown();
            if (!isBDown && _wasBDown) _mouseSimulator.RightButtonUp();
            _wasBDown = isBDown;
        }

        //Mapping hanh dong lan chuot
        private void Scroll(State state)
        {
            //if(state.Gamepad.LeftTrigger != 0)
            //{
            //    var x = state.Gamepad.RightThumbX / 10_000;
            //    var y = state.Gamepad.RightThumbY / 10_000;
            //    _mouseSimulator.HorizontalScroll(x);
            //    _mouseSimulator.VerticalScroll(y);
            //}

            var x = state.Gamepad.LeftThumbX / 10_000;
            var y = state.Gamepad.LeftThumbY / 10_000;
            _mouseSimulator.HorizontalScroll(x);
            _mouseSimulator.VerticalScroll(y);
        }

        //Mapping hanh dong di chuyen chuot
        public void Movement(State state)
        {
            var x = state.Gamepad.RightThumbX / 2_000;
            var y = state.Gamepad.RightThumbY / 2_000;
            _mouseSimulator.MoveMouseBy(x, -y);
        }

        public void getState(State state)
        {
            state.Gamepad.Buttons.HasFlag(GamepadButtonFlags.A);
        }

        public static bool IsKeyPressed(ConsoleKey key)
        {
            return Console.KeyAvailable && Console.ReadKey(true).Key == key;
        }
    }
}