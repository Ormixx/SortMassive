using System;
//Работу выполнил Рязанов А.В. ПИНб-21
public class Program //Главный класс
{
    public static void Main(string[] args)
    {
        Random random = new Random();
        int size = 100;
        IArrayOperations arrayManipulator = new SortedArray(size);

        for (int i = 0; i < size; i++)
        {
            arrayManipulator.Add(random.Next(50));
        }

        arrayManipulator.Display();

        long randomValueToSearch = random.Next(50);
        if (arrayManipulator.Contains(randomValueToSearch))
        {
            Console.WriteLine($"Значение было найдено: {randomValueToSearch}");
        }
        else
        {
            Console.WriteLine($"Не удалось найти значение: {randomValueToSearch}");
        }

        Console.WriteLine($"Минимальное значение: {arrayManipulator.GetMin()}");
        Console.WriteLine($"Максимальное значение: {arrayManipulator.GetMax()}");
    }
}

internal interface IArrayOperations //Интерфейс
{
    bool Contains(long searchValue);
    void Add(long value);
    bool Delete(long value);
    void Display();
    int GetSize();
    long GetMin();
    long GetMax();
}

internal class SortedArray : IArrayOperations //Класс реализации интерфейса
{
    private SortedLongArray _sortedArray;
    private int _count;

    public SortedArray(int size)
    {
        _sortedArray = new SortedLongArray(size);
        _count = 0;
    }

    public bool Contains(long value) => _sortedArray.Contains(value);

    public void Add(long value)
    {
        if (_sortedArray.Insert(value))
        {
            _count++;
        }
    }

    public bool Delete(long value)
    {
        bool delete = _sortedArray.Delete(value);
        if (delete) _count--;
        return delete;
    }

    public void Display()
    {
        for (int i = 0; i < _count; i++)
        {
            Console.Write(_sortedArray[i] + " ");
        }
        Console.WriteLine();
    }

    public int GetSize() => _count;

    public long GetMin() => _sortedArray.GetMin();

    public long GetMax() => _sortedArray.GetMax();
}

internal class SortedLongArray //Класс реализации поиска в упорядоченном массиве
{
    private long[] _array;
    private int _count;

    public SortedLongArray(int size)
    {
        _array = new long[size];
        _count = 0;
    }

    public bool Contains(long searchValue)
    {
        int lowerBound = 0;
        int upperBound = _count - 1;

        while (lowerBound <= upperBound)
        {
            int currentIndex = (lowerBound + upperBound) / 2;
            if (_array[currentIndex] == searchValue)
                return true;
            else if (_array[currentIndex] < searchValue)
                lowerBound = currentIndex + 1;
            else
                upperBound = currentIndex - 1;
        }
        return false;
    }

    public bool Insert(long value)
    {
        if (_count == _array.Length) return false; 

        int i;
        for (i = 0; i < _count; i++)
        {
            if (_array[i] == value)
                return false; 
            else if (_array[i] > value)
                break;
        }

        for (int j = _count; j > i; j--)
        {
            _array[j] = _array[j - 1];
        }
        _array[i] = value;
        _count++;
        return true;
    }

    public bool Delete(long value)
    {
        int index = Array.BinarySearch(_array, 0, _count, value);
        if (index < 0) return false;

        for (int j = index; j < _count - 1; j++)
        {
            _array[j] = _array[j + 1];
        }
        _count--;
        return true;
    }

    public long GetMin()
    {
        if (_count == 0) throw new InvalidOperationException("Array is empty");
        return _array[0];
    }

    public long GetMax()
    {
        if (_count == 0) throw new InvalidOperationException("Array is empty");
        return _array[_count - 1];
    }

    public long this[int index] => _array[index]; // Индексатор для доступа к элементам
}