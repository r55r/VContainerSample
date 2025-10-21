using JetBrains.Annotations;
using VContainer.Unity;

namespace tutorial
{
    [UsedImplicitly]
    public class GamePresenter : IStartable
    {
        private readonly HelloWorldService _helloWorldService;
        private readonly SayHelloButton _sayHelloButton;

        public GamePresenter(HelloWorldService helloWorldService,
            SayHelloButton sayHelloButton)
        {
            _helloWorldService = helloWorldService;
            _sayHelloButton = sayHelloButton;
        }

        public void Start()
        {
            _sayHelloButton.OnClick += () =>
            {
                _helloWorldService.SayHello();
            };
        }
    }
}