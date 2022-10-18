using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class Argument
{
	public Func<List<string>> possibleArguments { get; protected set; }
	public abstract Argument NextArgument(string key);

	public Argument next;

	public bool IsLast()
	{
		return next == null;
	}
}

public delegate bool ValidateArgument<T>(string argument, out T output);

public class CommandArgument<T> : Argument
{
	private T _value;
	public T Value
	{
		get
		{
			return _value;
		}
		private set
		{
			_value = value;
		}
	}
	private Dictionary<string, T> dict;
	private Action onCall;

	string errorMessage;

	ValidateArgument<T> validateInput;
	
	public CommandArgument(Argument next, Dictionary<string, T> arguments, string errorMessage = "")
	{
		this.errorMessage = errorMessage;
		List<string> args = new List<string>(arguments.Keys);
		args.Sort();
		possibleArguments = () => args;
		dict = arguments;
		this.next = next;
		onCall = null;
		validateInput = null;
	}

	public CommandArgument(Argument next, Func<List<string>> arguments, string errorMessage = "")
	{
		this.errorMessage = errorMessage;
		possibleArguments = () => { List<string> args = arguments?.Invoke(); args?.Sort(); return args; } ;
		this.next = next;
		onCall = null;
		validateInput = null;
	}

	public CommandArgument(Argument next, ValidateArgument<T> validateInput, string errorMessage = "")
	{
		dict = null;
		this.next = next;
		onCall = null;
		this.validateInput = validateInput;
		this.errorMessage = errorMessage;
	}
	
	public CommandArgument(Action onCall, Dictionary<string, T> arguments, string errorMessage = "")
	{
		List<string> args = new List<string>(arguments.Keys);
		args.Sort();
		possibleArguments = () => args;
		dict = arguments;
		this.next = null;
		this.onCall = onCall;
		this.errorMessage = errorMessage;
		validateInput = null;
	}


	public CommandArgument(Action onCall, Func<List<string>> arguments, string errorMessage = "")
	{
		possibleArguments = () => { List<string> args = arguments?.Invoke(); args?.Sort();  return args; } ;
		this.next = null;
		this.onCall = onCall;
		this.errorMessage = errorMessage;
		validateInput = null;
	}

	public CommandArgument(Action onCall, ValidateArgument<T> validateInput, string errorMessage = "")
	{
		dict = null;
		this.next = null;
		this.onCall = onCall;
		this.validateInput = validateInput;
		this.errorMessage = errorMessage;
	}

	public override Argument NextArgument(string thisArgument)
	{
		if ((dict == null || dict.TryGetValue(thisArgument, out _value)) && (validateInput == null || validateInput(thisArgument, out _value)))
		{
			onCall?.Invoke();
			return next;
		}
		if (errorMessage != "")
		{
			Console.SendMessageToChat("Invalid argument: '" + thisArgument + "'" + ", " + errorMessage, Color.red);
		}
		else
		{
			Console.SendMessageToChat("Invalid argument: '" + thisArgument + "'.", Color.red);
		}
		return null;
	}

}

public abstract class Command : IComparable<Command>
{
	public int CompareTo(Command other)
	{
		return string.Compare(Name(), other.Name());
	}

	public abstract string Name();
	public abstract string Usage();
	public abstract Argument Call();
	public bool HasArguments { get; protected set; } = true;
}

public class Command_color : Command
{
	Dictionary<string, Color> colorArguments = new Dictionary<string, Color>()
	{
		["red"] = Color.red,
		["blue"] = Color.blue,
		["green"] = Color.green,
	};
	Dictionary<string, int> numberArguments = new Dictionary<string, int>()
	{
		["1"] = 1,
		["2"] = 2,
		["3"] = 3,
	};

	CommandArgument<Color> colorArg;
	CommandArgument<int> numberArg;
	CommandArgument<string> nameArg;

	public Command_color()
	{
		nameArg = new CommandArgument<string>(Finish, (string s, out string output) =>
		{
			output = "";
			for (int i = 0; i < s.Length; i++)
			{
				if (!char.IsLetter(s[i])) return false;
			}
			output = s;
			return true;
		}, "Only Letters Allowed!");
		numberArg = new CommandArgument<int>(nameArg, numberArguments, "Valid values: 1 - 3.");
		colorArg = new CommandArgument<Color>(numberArg, colorArguments, "Valid values: red, green, blue.");
	}

	private void Finish()
	{
		Debugging.Log(colorArg.Value.ToString() + " " + numberArg.Value + " " + nameArg.Value);
	}

	public override Argument Call() => colorArg;

	public override string Name() => "/color";
	public override string Usage() => "/color [color] [number] [string]";
}

public class Command_timescale : Command
{
	CommandArgument<float> valueArg;

	public Command_timescale()
	{
		valueArg = new CommandArgument<float>(Finish, (string argument, out float output) =>
		{
			if(float.TryParse(argument, out output))
			{
				if (output < 0) return false;
				return true;
			}
			return false;
		}, "Value must be >= 0.");
	}

	private void Finish()
	{
		Console.SendMessageToChat("Timescale set to: " + valueArg.Value);
		Time.timeScale = valueArg.Value;
	}

	public override Argument Call() => valueArg;

	public override string Name() => "/timescale";

	public override string Usage() => "/timescale [float]";
}

public class Command_reset : Command
{
	public Command_reset()
	{
		HasArguments = false;
	}

	public override Argument Call()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		return null;
	}

	public override string Name() => "/reset";

	public override string Usage() => "/reset";
}

public class Command_loadscene : Command
{
	CommandArgument<string> valueArg;

	Dictionary<string, string> sceneArguments;

	public Command_loadscene()
	{
		sceneArguments = new Dictionary<string, string>();
		for(int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
		{
			string sceneName = System.IO.Path.GetFileNameWithoutExtension( UnityEngine.SceneManagement.SceneUtility.GetScenePathByBuildIndex( i ) );
			sceneArguments.Add(sceneName, sceneName);
			sceneArguments.Add(i.ToString(), sceneName);
		}
		valueArg = new CommandArgument<string>(Finish, sceneArguments, "Please enter valid buildIndex or sceneName.");
	}

	private void Finish()
	{
		Console.SendMessageToChat("Loading scene '" + valueArg.Value + "'.");
		SceneManager.LoadScene(valueArg.Value);
	}

	public override Argument Call() => valueArg;

	public override string Name() => "/loadscene";

	public override string Usage() => "/loadscene [buildIndex / sceneName]";
}

/*
public class Command_function : Command
{
	CommandArgument<string> actionName;
	CommandArgument<string> functionName;

	Dictionary<string, string> actionNameArguments = new Dictionary<string, string>()
	{
		["destroy"] = "destroy",
		["pause"] = "pause",
		["resume"] = "resume",
	};

	public Command_function()
	{
		functionName = new CommandArgument<string>(Finish, () =>
		{
			List<string> arguments = null;
			var list = FunctionBase.GetAllFunctions();
			if (list != null && list.Count != 0)
			{
				arguments = new List<string>();
				for (int i = 0; i < list.Count; i++)
				{
					arguments.Add(list[i].FunctionName);
				}
			}
			return arguments;
		}, "Chose valid function name.");
		actionName = new CommandArgument<string>(functionName, actionNameArguments);
	}

	private void Finish()
	{
		switch(actionName.Value)
		{
			case "destroy":
				FunctionBase.DestroyFunction(functionName.Value);
				break;
			case "pause":
				FunctionBase.Find(functionName.Value).Pause();
				break;
			case "resume":
				FunctionBase.Find(functionName.Value).Resume();
				break;
		}
	}

	public override Argument Call() => actionName;

	public override string Name() => "/function";

	public override string Usage() => "/function [action] [functionName]";
}
*/