using System;

namespace GenericClassImplementation
{
    class Program
    {
        static void Main(string[] args)
        {
            Merc tmp = new Merc(4, "PV");
            tmp.printMethod();
            Console.Read();
        }
    }

    class GenericVehicle
    {
        public GenericVehicle(int tyre, string purpose)
        {

        }
        public virtual void printMethod()
        {
            Console.WriteLine("Generic");
        }
    }

    class Car: GenericVehicle
    {
        public Car(int tyre, string purpose): base(tyre,purpose)
        {

        }
        public override void printMethod()
        {
            Console.WriteLine("Car");
        }
    }

    class Merc : Car
    {
        public Merc(int tyre, string purpose) : base(tyre, purpose)
        {

        }
        
    }
}
