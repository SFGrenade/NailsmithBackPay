using System.Linq;
using HutongGames.PlayMaker;
using Modding;

namespace NailsmithBackPay;

public static class Util
{
    public static void SetAttr<TSelf, TVal>(this TSelf o, string fieldname, TVal value)
    {
        ReflectionHelper.SetField(o, fieldname, value);
    }

    public static TAction GetAction<TAction>(this PlayMakerFSM self, string state, int index) where TAction : FsmStateAction
    {
        return (TAction) self.FsmStates.First(s => s.Name == state).Actions[index];
    }
}