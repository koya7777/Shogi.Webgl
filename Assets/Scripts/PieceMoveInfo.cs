using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceMoveInfo
{
	public Address MoveFrom { get; private set; } = Address.INVALID_ADDRESS;
	public Address MoveTo { get; private set; } = Address.INVALID_ADDRESS;
	public PieceType PieceType { get; private set; } = PieceType.Empty;
	public Square SelectingSquare { get; private set; }

	public bool IsSelecting => SelectingSquare != null;
	public bool IsMoveAcquiredPiece => IsSelecting && !MoveFrom.IsValid();

	public PieceMoveInfo() { }
	public PieceMoveInfo(Address moveFrom, Address moveTo, PieceType pieceType)
	{
		MoveFrom = moveFrom;
		MoveTo = moveTo;
		PieceType = pieceType;
	}

	public void SetMoveFrom(Square square)
	{
		MoveFrom = square.Address;
		PieceType = square.PieceType;
		SelectingSquare = square;
	}
	public void SetMoveTo(Address to) => MoveTo = to;
	public bool IsSameAddress(Address to) => MoveFrom == to;

	public void Reset()
	{
		MoveFrom = Address.INVALID_ADDRESS;
		MoveTo = Address.INVALID_ADDRESS;
		PieceType = PieceType.OutOfBoard;
		SelectingSquare = null;
	}
}
