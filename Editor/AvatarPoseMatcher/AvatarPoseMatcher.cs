#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace colorlesscolor.unitytools.avatar.avatarposematcher
{
    public class AvatarPoseMatcher : EditorWindow
    {
        private Animator buggedAvatarAnimator;
        private Animator targetAvatarAnimator;

        [MenuItem("Tools/Avatar Pose Matcher")]
        static void init()
        {
            AvatarPoseMatcher window = (AvatarPoseMatcher)GetWindow(typeof(AvatarPoseMatcher), false, "Avatar Pose Matcher");
            window.Show();
        }

        private void OnGUI()
        {
            GUILayout.Label("Avatar Pose Matcher", new GUIStyle(EditorStyles.label)
            {
                alignment = TextAnchor.MiddleCenter,
                wordWrap = true,
                fontSize = 12
            });

            GUILayout.Space(10);

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Bug report: ");
            if (EditorGUILayout.LinkButton("github.com/ColorlessColor/Avatar_Pose_Matcher"))
            {
                Application.OpenURL("https://github.com/ColorlessColor/Avatar_Pose_Matcher");
            }
            EditorGUILayout.EndHorizontal();

            GUILayout.Space(20);

            buggedAvatarAnimator = (Animator)EditorGUILayout.ObjectField("Bugged Avatar", buggedAvatarAnimator, typeof(Animator), true);
            targetAvatarAnimator = (Animator)EditorGUILayout.ObjectField("Target Avatar", targetAvatarAnimator, typeof(Animator), true);

            if (
                buggedAvatarAnimator != null
                && targetAvatarAnimator != null
                && buggedAvatarAnimator == targetAvatarAnimator
            )
            {
                targetAvatarAnimator = null;
            }

            if (
                buggedAvatarAnimator != null
                && targetAvatarAnimator != null
                && buggedAvatarAnimator.avatar != null
                && targetAvatarAnimator.avatar != null
                && buggedAvatarAnimator != targetAvatarAnimator
                && GUILayout.Button("Match")
            )
            {
                targetAvatarAnimator.gameObject.transform.localScale = buggedAvatarAnimator.gameObject.transform.localScale;
                targetAvatarAnimator.gameObject.transform.SetLocalPositionAndRotation(
                    buggedAvatarAnimator.gameObject.transform.localPosition,
                    buggedAvatarAnimator.gameObject.transform.localRotation
                );

                for (int i = 0; i < (int)HumanBodyBones.LastBone; i++)
                {
                    var bone = (HumanBodyBones)i;
                    try
                    {
                        var buggedBone = buggedAvatarAnimator.GetBoneTransform(bone);
                        var targetBone = targetAvatarAnimator.GetBoneTransform(bone);
                        if (buggedBone != null && targetBone != null)
                        {
                            Debug.Log($"[Avatar Pose Matcher] Bugged Avatar: {bone} fixed");
                            buggedBone.SetLocalPositionAndRotation(targetBone.localPosition, targetBone.localRotation);
                        }
                        else
                        {
                            if (targetBone == null)
                            {
                                Debug.LogWarning($"[Avatar Pose Matcher] Target Avatar: {bone} not found");
                            }
                            if (buggedBone == null)
                            {
                                Debug.LogWarning($"[Avatar Pose Matcher] Bugged Avatar: {bone} not found");
                            }
                        }
                    }
                    catch
                    {
                        Debug.LogWarning($"[Avatar Pose Matcher] An error occurred while get bone: {bone}");
                        continue;
                    }
                }
            }
        }
    }
}
#endif