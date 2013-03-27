Slognals
===========

A C# event API that uses Reactive Extensions with a Qt-inspired signal/slot syntax targetting the .NET 4.0 framework

Built against version 1.0.10621 of Reactive Extensions.

Features
--------
* Provides compile-time checking to ensure compatible signals and slots are being connected and disconnected
* Allows signals and slots that take up to 4 arguments
* Leverages portions of Reactive Extensions to allow for Where()-type filtering during connecting
* Any void-returning method can be used as a slot, no need to declare it as such

How to use: 

    // Example class that has a signal.

	class NumberPairGenerator
	{
			private Signal<int, int> _newPairSignal = new Signal<int, int>();

			public Signal<int, int> Signal_NewPair
			{
				get
				{
					return _newPairSignal;
				}
			}

			public void GeneratePair()
			{
				Random random = new Random();
				int randomNumber1 = random.Next(0, 100);
				int randomNumber2 = random.Next(0, 100);
				_newPairSignal.emit(randomNumber1, randomNumber2);
			}
	}

	// Example class that has a slot we want to use.

	class NumberPairWriter
	{
			public void WritePair(int val1, int val2)
			{
				Console.WriteLine("First num: {0}, second num: {1}", val1, val2);
			}
	}

	// Example usage.

	var generator = new NumberPairGenerator();
	var writer = new NumberPairWriter();

	Signal<int, int>.connect(generator.Signal_NewPair, writer.WritePair);
	generator.GeneratePair();

	Signal<int, int>.disconnect(generator.Signal_NewPair, writer.WritePair);

	Signal<int, int>.connect(generator.Signal_NewPair, writer.WritePair, (x, y) => x > 50 && y > 50);

	generator.GeneratePair();
