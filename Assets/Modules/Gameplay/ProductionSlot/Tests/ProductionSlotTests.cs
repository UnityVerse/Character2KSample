using Game.Gameplay;
using NUnit.Framework;

namespace Modules.Gameplay
{
    [TestFixture]
    public sealed class ProductionSlotTests
    {
        [Test]
        public void StartProduction()
        {
            //Arrange:
            ProductionSlot<object> slot = new ProductionSlot<object>();
            object target = new object();
            bool wasEvent = false;

            //Act:
            slot.OnStarted += _ => wasEvent = true;
            slot.Start(target, 5);

            //Assert:
            Assert.IsTrue(wasEvent);
            Assert.IsTrue(slot.IsPlaying);
            Assert.AreEqual(target, slot.Target);
        }

        [Test]
        public void WhenGetProgressOfNotStartedProductionThenReturnZero()
        {
            ProductionSlot<object> slot = new ProductionSlot<object>();
            Assert.AreEqual(0, slot.GetProgress());
        }

        [Test]
        public void WhenTickNotStartedProductionThenNothing()
        {
            //Arrange:
            ProductionSlot<object> slot = new ProductionSlot<object>();
            bool wasTimeEvent = false;
            bool wasProgressEvent = false;

            //Act:
            slot.OnCurrentTimeChanged += _ => wasTimeEvent = true;
            slot.OnProgressChanged += _ => wasProgressEvent = true;
            slot.Tick(deltaTime: 0.5f);

            //Assert:
            Assert.IsFalse(slot.IsPlaying);
            Assert.IsFalse(wasTimeEvent);
            Assert.IsFalse(wasProgressEvent);

            Assert.AreEqual(0, slot.CurrentTime);
            Assert.AreEqual(0, slot.GetProgress());
        }

        [Test]
        public void WhenTickProductionThenProgressChanged()
        {
            //Arrange:
            ProductionSlot<object> slot = new ProductionSlot<object>();
            object target = new object();
            float progress = -1;

            //Act:
            slot.Start(target, 5);
            slot.OnProgressChanged += p => progress = p;
            slot.Tick(deltaTime: 1);

            //Assert:
            Assert.AreEqual(0.2f, progress, float.Epsilon);
            Assert.AreEqual(0.2f, slot.GetProgress(), float.Epsilon);
        }

        [Test]
        public void TickWithMultiplier()
        {
            //Arrange:
            ProductionSlot<object> slot = new ProductionSlot<object>(multiplier: 3);
            object target = new object();

            //Act:
            slot.Start(target, 15);
            
            for (int i = 0; i < 4; i++)
            {
                slot.Tick(deltaTime: 1);
                Assert.AreEqual(3 * (i + 1), slot.CurrentTime, float.Epsilon);
            }

            //Assert:
            Assert.AreEqual(0.8f, slot.GetProgress(), float.Epsilon);
        }

        [Test]
        public void FinishTraining()
        {
            //Arrange:
            ProductionSlot<object> slot = new ProductionSlot<object>();
            object target = new object();
            object completedTarget = false;

            //Act:
            slot.Start(target, 4);
            slot.OnFinished += ct => completedTarget = ct;

            for (int i = 0; i < 20; i++)
            {
                slot.Tick(deltaTime: 0.2f);
            }

            //Assert:
            Assert.AreEqual(target, completedTarget);
            Assert.AreEqual(1, slot.GetProgress());
            Assert.AreEqual(4, slot.CurrentTime);
            Assert.AreEqual(4, slot.TargetDuration);
            Assert.AreEqual(target, slot.Target);
            Assert.IsFalse(slot.IsPlaying);
            Assert.IsFalse(slot.IsPaused);
            Assert.IsTrue(slot.IsFinished);
        }

        [Test]
        public void PauseProduction()
        {
            //Arrange:
            ProductionSlot<object> slot = new ProductionSlot<object>();
            object target = new object();
            bool wasPause = false;

            //Act:
            slot.Start(target, 1);
            slot.OnPaused += _ => wasPause = true;
            slot.Pause();

            //Assert:
            Assert.IsTrue(wasPause);
            Assert.IsTrue(slot.IsPaused);
        }

        [Test]
        public void WhenTickInPausedThenNothing()
        {
            //Arrange:
            ProductionSlot<object> slot = new ProductionSlot<object>();
            object target = new object();
            bool progressChanged = false;
            bool timeChanged = false;
            bool completed = false;

            //Act:
            slot.Start(target, 0.8f);
            slot.Tick(0.2f);
            slot.OnProgressChanged += _ => progressChanged = true;
            slot.OnCurrentTimeChanged += _ => timeChanged = true;
            slot.OnFinished += _ => completed = true;

            slot.Pause();

            for (int i = 0; i < 5; i++)
            {
                slot.Tick(deltaTime: 0.2f);
            }

            //Assert:
            Assert.IsTrue(slot.IsPaused);
            Assert.AreEqual(0.2f, slot.CurrentTime);
            Assert.AreEqual(0.25f, slot.GetProgress(), float.Epsilon);

            Assert.IsFalse(progressChanged);
            Assert.IsFalse(timeChanged);
            Assert.IsFalse(completed);
        }

        [Test]
        public void WhenPauseProductionThatNotStartedThenNothing()
        {
            //Arrange:
            ProductionSlot<object> slot = new ProductionSlot<object>();
            bool wasPause = false;

            //Act:
            slot.OnPaused += _ => wasPause = true;
            slot.Pause();

            //Assert:
            Assert.IsFalse(wasPause);
            Assert.IsFalse(slot.IsPaused);
        }

        [Test]
        public void ResumeProduction()
        {
            //Arrange:
            ProductionSlot<object> slot = new ProductionSlot<object>();
            object target = new object();
            bool wasResume = false;

            //Act:
            slot.OnResumed += _ => wasResume = true;
            slot.Start(target, 1);
            slot.Pause();
            Assert.IsTrue(slot.IsPaused);
            slot.Resume();

            //Assert:
            Assert.IsTrue(wasResume);
            Assert.IsTrue(!slot.IsPaused);
        }

        [Test]
        public void WhenResumeProductionThatNotStartedThenNothing()
        {
            //Arrange:
            ProductionSlot<object> slot = new ProductionSlot<object>();
            bool wasResume = false;

            //Act:
            slot.OnResumed += _ => wasResume = true;
            slot.Resume();

            //Assert:
            Assert.IsFalse(wasResume);
            Assert.IsFalse(slot.IsPlaying);
            Assert.IsFalse(slot.IsPaused);
        }

        [Test]
        public void WhenTickProductionThenCurrentTimeChanged()
        {
            //Arrange:
            ProductionSlot<object> slot = new ProductionSlot<object>();
            object target = new object();
            float currentTime = -1;

            //Act:
            slot.Start(target, 5);
            slot.OnCurrentTimeChanged += t => currentTime = t;
            slot.Tick(deltaTime: 0.5f);

            //Assert:
            Assert.AreEqual(0.5f, currentTime);
            Assert.AreEqual(0.5f, slot.CurrentTime);
        }

        [Test]
        public void WhenStartProductionThatAlreadyStartedThenFailed()
        {
            //Arrange:
            ProductionSlot<object> slot = new ProductionSlot<object>();
            bool wasEvent = false;
            object target1 = new object();
            object target2 = new object();

            //Act:
            slot.Start(target1, 5);
            slot.OnStarted += _ => wasEvent = true;
            slot.Start(target2, 10);

            //Assert:
            Assert.IsFalse(wasEvent);
            Assert.AreEqual(target1, slot.Target);
            Assert.AreEqual(5, slot.TargetDuration);
            Assert.AreNotEqual(target2, slot.Target);
        }

        [Test]
        public void CancelProduction()
        {
            //Arrange:
            ProductionSlot<object> slot = new ProductionSlot<object>();
            object target = new object();
            object canceledTarget = null;

            //Act:
            slot.Start(target, 5);
            slot.OnCanceled += t => canceledTarget = t;
            slot.Cancel();

            //Assert:
            Assert.AreEqual(canceledTarget, target);
            Assert.IsNull(slot.Target);
            Assert.IsFalse(slot.IsPlaying);
            Assert.AreEqual(0, slot.TargetDuration);
            Assert.AreEqual(0, slot.CurrentTime);
        }

        [Test]
        public void ForceStartProduction()
        {
            //Arrange:
            ProductionSlot<object> slot = new ProductionSlot<object>();
            object target1 = new object();
            object target2 = new object();

            object canceledTarget = null;
            object startedTarget = null;


            //Act:
            slot.Start(target1, 5);
            slot.OnCanceled += tc => canceledTarget = tc;
            slot.OnStarted += ts => startedTarget = ts;
            slot.ForceStart(target2, 10);

            //Assert:
            Assert.AreEqual(target1, canceledTarget);
            Assert.AreEqual(target2, startedTarget);

            Assert.AreEqual(target2, slot.Target);
            Assert.AreEqual(10, slot.TargetDuration);
            Assert.AreEqual(0, slot.CurrentTime);
            Assert.IsTrue(slot.IsPlaying);
        }

        [Test]
        public void WhenCancelNotStartedProductionThenNoEvent()
        {
            //Arrange:
            ProductionSlot<object> slot = new ProductionSlot<object>();
            bool wasEvent = false;

            //Act:
            slot.OnCanceled += _ => wasEvent = true;
            slot.Cancel();

            //Assert:
            Assert.IsFalse(wasEvent);
        }

        [Test]
        public void WhenResumeProductionThatIsNotPausedThenNothing()
        {
            //Arrange:
            ProductionSlot<object> slot = new ProductionSlot<object>();
            object target = new object();
            bool wasResume = false;

            //Act:
            slot.OnResumed += _ => wasResume = true;
            slot.Start(target, 1);
            Assert.IsTrue(!slot.IsPaused);

            slot.Resume();

            //Assert:
            Assert.IsFalse(wasResume);
        }

        [Test]
        public void WhenCancelPausedProductionThenProductionWillNotPaused()
        {
            //Arrange:
            ProductionSlot<object> slot = new ProductionSlot<object>();
            object target = new object();
            object canceledTarget = null;

            //Act:
            slot.Start(target, 5);
            slot.Pause();
            Assert.IsTrue(slot.IsPaused);

            slot.OnCanceled += t => canceledTarget = t;
            slot.Cancel();

            //Assert:
            Assert.AreEqual(canceledTarget, target);
            Assert.IsNull(slot.Target);
            Assert.IsFalse(slot.IsPaused);
            Assert.IsFalse(slot.IsPlaying);
            Assert.AreEqual(0, slot.TargetDuration);
            Assert.AreEqual(0, slot.CurrentTime);
        }
    }
}