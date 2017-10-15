﻿using System;
using System.Collections;
using System.Collections.Generic;

// https://github.com/Real-Serious-Games/Fluent-State-Machine

public interface IState
{
	IState Parent { get; set; }

	void ChangeState(string stateName);
	void PushState(string stateName);
	void PopSate();
	void Update(float dt);
	void Enter();
	void Exit();
	// Trigger an event on this state or one of its children
	void TriggerEvent(string name);
	void TriggerEvent(string name, EventArgs eventArgs);
}

public abstract class AbstractState : IState
{
	private struct Condition
	{
		public Func<bool> Predicate;
		public Action Act;
	}

	private Action onEnter;
	private Action<float> onUpdate;
	private Action onExit;

	private readonly List<Condition> conditions = new List<Condition>();
	// Stack of active child states
	private readonly Stack<IState> activeChildren = new Stack<IState>();
	private readonly Dictionary<string, IState> children = new Dictionary<string, IState>();
	private readonly Dictionary<string, Action<EventArgs>> events = new Dictionary<string, Action<EventArgs>>();

	public IState Parent { get; set; }

	// Pops the current state from the stack and pushes the specified one on
	public void ChangeState(string stateName)
	{
		IState newState;
		if (!children.TryGetValue (stateName, out newState)) 
		{
			throw new ApplicationException("Tried to change to state \"" + stateName + "\", but it is not in the list of children.");
		}

		if (activeChildren.Count > 0) 
		{
			activeChildren.Pop().Exit ();
		}

		activeChildren.Push (newState);
		newState.Enter ();
	}

	// Push another state from the existing dictionary of children to the top of the state stack.
	public void PushState(string stateName)
	{
		// Find the new state and add it
		IState newState;
		if (!children.TryGetValue(stateName, out newState))
		{
			throw new ApplicationException("Tried to change to state \"" + stateName + "\", but it is not in the list of children.");
		}
		activeChildren.Push(newState);
		newState.Enter();
	}

	// Remove the current state from the active state stack and activate the state immediately beneath it.
	public void PopState()
	{
		// Exit and pop the current state
		if (activeChildren.Count > 0)
		{
			activeChildren.Pop().Exit();
		}
		else
		{
			throw new ApplicationException("PopState called on state with no active children to pop.");
		}
	}

	public void Update(float dt)
	{
		if (activeChildren.Count > 0) 
		{
			activeChildren.Peek ().Update (dt);
			return;
		}

		if (onUpdate != null) 
		{
			onUpdate (dt);
		}

		for (int i = 0; i < conditions.Count; i++) 
		{
			if (conditions [i].Predicate ()) 
			{
				conditions [i].Act ();
			}
		}
	}

	public void AddChild(IState newState, string stateName)
	{
		try
		{
			children.Add(stateName, newState);
			newState.Parent = this;
		}
		catch (ArgumentException)
		{
			throw new ApplicationException("State with name \"" + stateName + "\" already exists in list of children.");
		}
	}
}