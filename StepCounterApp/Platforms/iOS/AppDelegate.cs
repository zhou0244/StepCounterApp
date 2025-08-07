using Foundation;
using UIKit;
using CoreMotion;

namespace StepCounterApp;

[Register("AppDelegate")]
public class AppDelegate : MauiUIApplicationDelegate
{
    CMMotionActivityManager activityManager;
    CMPedometer pedometer;
    static int stepCount = 0;

    protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
    public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
    {
        var pedometer = new CMPedometer();

        if (CMPedometer.IsStepCountingAvailable)
        {
            var startDate = NSDate.Now;
            try
            {
                pedometer.StartPedometerUpdates(NSDate.Now, (data, error) =>
                {
                    Console.WriteLine($"CMPedometer update: error={error}, steps={data?.NumberOfSteps}");

                    if (error == null && data != null)
                    {
                        int stepCount = data.NumberOfSteps?.Int32Value ?? 0;

                        MainThread.BeginInvokeOnMainThread(() =>
                        {
                            MessagingCenter.Send<object, int>(this, "StepUpdated", stepCount);
                        });
                    }
                });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            
        }

        return base.FinishedLaunching(application, launchOptions);
    }

}