using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Address
{
	public static readonly Address INVALID_ADDRESS = new Address(-1, -1);

	public int X { get; private set; }
	public int Y { get; private set; }

	public Address(int x, int y)
	{
		X = x;
		Y = y;
	}

	public Address(Address address)
	{
		X = address.X;
		Y = address.Y;
	}

	/// <summary>
	/// 将棋盤の中にあればtrueを返す
	/// </summary>
	/// <returns></returns>
	public bool IsValid()
	{
		if (X > 9 || X < 1)
		{
			return false;
		}

		if (Y > 9 || Y < 1)
		{
			return false;
		}

		return true;
	}

	public static Address operator +(Address a, Address b) => new Address(a.X + b.X, a.Y + b.Y);
	public static bool operator ==(Address a, Address b) => (a.X == b.X && a.Y == b.Y);
	public static bool operator !=(Address a, Address b) => (a.X != b.X || a.Y != b.Y);
	public static Address operator *(Address a, int b) => new Address(a.X * b, a.Y * b);
	public static Address operator *(Address a, Address b) => new Address(a.X * b.X, a.Y * b.Y);

	// 以下2行が無いと CS0660 の Warning が出るため記載
	//public override bool Equals(object o) => true;
	//public override int GetHashCode() => 0;

	//objと自分自身が等価のときはtrueを返す
	public override bool Equals(System.Object obj)
	{
		//objがnullか、型が違うときは、等価でない
		if (obj == null || this.GetType() != obj.GetType())
		{
			return false;
		}

        return (this == (Address)obj);
    }

	//Equalsがtrueを返すときに同じ値を返す
	public override int GetHashCode()
	{
		return this.X ^ this.Y;
	}
}
