using UnityEngine;

public static class ExtensionMethods
{
    public static void SetBoolWithParameterCheck(this Animator _animator, string _parameter, AnimatorControllerParameterType _type, bool _value)
    {
        if (_animator.HasParameterOfType(_parameter, _type))
        {
            _animator.SetBool(_parameter, _value);
            return;
        }
        else return;
    }

    public static bool HasParameterOfType(this Animator self, string name, AnimatorControllerParameterType type)
    {
        if (name == null || name == "") { return false; }
        AnimatorControllerParameter[] parameters = self.parameters;
        foreach (AnimatorControllerParameter currParam in parameters)
        {
            if (currParam.type == type && currParam.name == name)
            {
                return true;
            }
        }
        return false;
    }
}
