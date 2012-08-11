using System;
using YADA.Acceptance.StepDefinations;

namespace PerformanceTuningProject
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                for (int i = 0; i < 15; i++)
                {
                    try
                    {
                        var reading = new SmallTableReading();

                        reading.GivenIHaveATablePopulatedWithOver100000Rows();
                        reading.WhenUsingAStoreProcedureTestToReadTheRecords("Sales.LargeRowTest");
                        reading.ThenTheOperationShouldHappenInLessThanMS(16);
                    }
                    catch (Exception ex)
                    {
                        Helpers.WriteErrorToConsole(ex);
                    }
                }
            }
            catch (Exception ex)
            {
                Helpers.WriteErrorToConsole(ex);
            }
        }
    }
}