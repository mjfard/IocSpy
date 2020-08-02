# IocSpy
An integration of IoC containers and a spying framework with fluent API

this tool genenerates spies, IocConfig and TestIocConfig for you.

Example of Spy setup for a test:

        public override void SpySetUp()
        {
            Interface<IDao<MainPOCO>>()
                .Method(i => nameof(i.Update))
                .Input_0_<MainPOCO>(poco => 
                    Check(poco.Level2One.ID != null))
                .CountShouldBe(1);

            Interface<IDbSet<Level3Many2POCO>>()
                .Method(i => nameof(i.Add))
                .CountShouldBe(1);

            Interface<IDbSet<Level3Many2POCO>>()
                .Method(i => nameof(i.Add))
                .CountShouldBe(1);
        }
