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
	/// �����Ղ̒��ɂ����true��Ԃ�
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

	// �ȉ�2�s�������� CS0660 �� Warning ���o�邽�ߋL��
	//public override bool Equals(object o) => true;
	//public override int GetHashCode() => 0;

	//obj�Ǝ������g�������̂Ƃ���true��Ԃ�
	public override bool Equals(System.Object obj)
	{
		//obj��null���A�^���Ⴄ�Ƃ��́A�����łȂ�
		if (obj == null || this.GetType() != obj.GetType())
		{
			return false;
		}

        return (this == (Address)obj);
    }

	//Equals��true��Ԃ��Ƃ��ɓ����l��Ԃ�
	public override int GetHashCode()
	{
		return this.X ^ this.Y;
	}
}
