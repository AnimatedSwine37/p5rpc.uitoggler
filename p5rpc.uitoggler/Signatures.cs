using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace p5rpc.uitoggler
{
    internal static class Signatures
    {
        private static readonly string[] MapHook =
        {
            "push rax",
            "push rbx",
            "mov rax, [rdx+0x78]",
            "mov rbx, rdx",
            "test rax, rax",
            "jz condition",
            "test byte [rax+0xC], 1",
            "jz runNormally",
            "label condition",
            "test byte [rdx+0x18], 4",
            "jnz runNormally",
            "pop rbx",
            "pop rax",
            "ret",
            "label runNormally",
            "pop rbx",
            "pop rax",
            // Original code
            "push rbx",
            "sub rsp, 0x20",
            "mov rax, 0x4000000000"
        };

        // Individual item sigs
        internal static readonly SigInfo RenderCursorSig = new SigInfo("4C 8B DC 53 48 81 EC D0 00 00 00", "Cursor", "ret");
        internal static readonly SigInfo RenderDateSig = new SigInfo("48 8B C4 53 55 56 57 41 56 48 81 EC D0 00 00 00", "Date", "ret");
        internal static readonly SigInfo RenderObjectiveSig = new SigInfo("4C 8B DC 55 57 49 8D 6B ?? 48 81 EC 68 01 00 00 8B 42 ??", "Objective", "ret");
        internal static readonly SigInfo RenderMapSig = new SigInfo("40 53 48 83 EC 20 48 B8 00 00 00 00 40 00 00 00", "Map", string.Join('\n', MapHook));

        // Buttom prompt sigs
        internal static readonly SigInfo RenderFieldButtonPromptsSig = new SigInfo("40 53 48 83 EC 20 F6 82 ?? ?? ?? ?? 01 48 8B DA 74 ?? E8 ?? ?? ?? ?? 66 83 0D ?? ?? ?? ?? 40 48 8B CB E8 ?? ?? ?? ?? 48 8B CB", "Field Buttons", "ret");
        internal static readonly SigInfo RenderSelectionMenuBackSig = new SigInfo("E8 ?? ?? ?? ?? 48 85 DB 74 ?? 48 8B 43 ?? 4C 8D 86 ?? ?? ?? ?? BA 01 00 00 00", "Selection Menu Back", "test rbx, rbx");
        internal static readonly SigInfo RenderSelectionMenuConfirmSig = new SigInfo("E8 ?? ?? ?? ?? 48 85 DB 0F 84 ?? ?? ?? ?? 48 8B 43 ?? 4C 8D 86 ?? ?? ?? ??", "Selection Menu Confirm", "test rbx, rbx");
        internal static readonly SigInfo RenderBattleButtonPromptsSig = new SigInfo("40 55 56 57 41 55 48 8D AC 24 ?? ?? ?? ??", "Battle Buttons", "ret");
        internal static readonly SigInfo RenderBattleBackConfirmSig = new SigInfo("40 55 53 56 57 48 8D 6C 24 ?? 48 81 EC 98 00 00 00", "Battle Back Confirm", "ret");
        internal static readonly SigInfo RenderBattlePersonaStatsPromptSig = new SigInfo("48 83 EC 58 F3 0F 10 84 24 ?? ?? ?? ?? 4D 8B C8", "Battle Persona Stats Prompt", "ret");
        internal static readonly SigInfo RenderCommandSubMenuPromptsSig = new SigInfo("48 83 EC 28 80 3D ?? ?? ?? ?? 00 74 ?? 80 3D ?? ?? ?? ?? 00 75 ??", "Command Sub Menu Prompts", "ret");
        internal static readonly SigInfo RenderPhoneCloseConfirmSig = new SigInfo("40 55 57 48 8D 6C 24 ?? 48 81 EC E8 00 00 00", "Phone Close Confirm", "ret");
        internal static readonly SigInfo RenderPhoneScrollBackSig = new SigInfo("48 89 5C 24 ?? 48 89 6C 24 ?? F3 0F 11 54 24 ??", "Phone Scroll Back", "ret");
        internal static readonly SigInfo RenderVideoViewingBackConfirmSig = new SigInfo("E8 ?? ?? ?? ?? 49 83 7C 24 ?? 00", "Video Viewing Back Confirm", "cmp qword [r12+10],00");
        internal static readonly SigInfo RenderMainMenuPromptsSig = new SigInfo("48 89 5C 24 ?? 57 48 83 EC 40 80 79 ?? 00 49 8B D8", "Main Menu Prompts", "ret");
        internal static readonly SigInfo RenderSettingsPromptsSig = new SigInfo("48 89 5C 24 ?? 57 48 83 EC 20 48 8B F9 B3 01", "Settings Prompts", "ret");
        internal static readonly SigInfo RenderCraftingPromptsSig = new SigInfo("48 8B C4 56 48 81 EC 00 01 00 00 0F 29 70 ?? 48 8B F1 F3 0F 10 71 ??", "Crafting Prompts", "ret");
        internal static readonly SigInfo RenderShopBackSig = new SigInfo("E8 ?? ?? ?? ?? 49 8B 5E ?? 48 85 DB 0F 84 ?? ?? ?? ??", "Shop Back", "mov rbx, [r14+0x50]");
        internal static readonly SigInfo RenderShopConfirmSig = new SigInfo("E8 ?? ?? ?? ?? 48 8B 0F BA 82 00 00 00", "Shop Confirm", "mov rcx, [rdi]");
        internal static readonly SigInfo RenderHoldupBreakFormationSig = new SigInfo("E8 ?? ?? ?? ?? 49 8B 47 ?? 48 8B 48 ?? 44 38 A1 ?? ?? ?? ??", "Holdup Break Formation", "mov rax, [rbx+0x48]");
        internal static readonly SigInfo RenderHoldupTalkSig = new SigInfo("F3 0F 11 85 ?? ?? ?? ?? E8 ?? ?? ?? ?? 49 8B 47 ??", "Holdup Talk", "movss [rbp+0x68],xmm2");
        internal static readonly SigInfo RenderHoldupTargetSig = new SigInfo("E8 ?? ?? ?? ?? BA 67 00 00 00 48 8B CB", "Holdup Target", "mov edx, 0x67");
        internal static readonly SigInfo RenderHoldupAOASig = new SigInfo("4C 8B DC 49 89 5B ?? 57 48 83 EC 70 48 81 C2 D8 02 00 00", "Holdup AOA", "ret");
        internal static readonly SigInfo RenderGunTargetSig = new SigInfo("E8 ?? ?? ?? ?? 48 85 DB 0F 84 ?? ?? ?? ?? 48 8B 7B ?? 48 8B 4F ?? 0F B6 41 ?? F6 D0 A8 01 75 ?? 83 61 ?? FE 48 8B 7B ?? 48 8B 4F ?? 0F B6 41 ?? F6 D0 A8 01 75 ?? 83 61 ?? FE 48 8B 7B ?? 80 3F 00", "Gun Target", "test rbx, rbx");
        internal static readonly SigInfo RenderSkillAimAnalyzeSig = new SigInfo("E8 ?? ?? ?? ?? F3 0F 58 3D ?? ?? ?? ?? F3 0F 11 7D ??", "Skill Analyze", "");
        internal static readonly SigInfo RenderSkillAimTargetSig = new SigInfo("E8 ?? ?? ?? ?? BA 67 00 00 00 48 8B CF", "Skill Target", "mov edx,0x67");
        internal static readonly SigInfo RenderPersonaDetailsPromptsSig = new SigInfo("40 53 48 83 EC 20 8B 0D ?? ?? ?? ?? 48 8B DA E8 ?? ?? ?? ?? 48 8B 43 ??", "Persona Details Prompts", "ret");
        internal static readonly SigInfo RenderResultsNextSig = new SigInfo("F3 0F 11 44 24 ?? E8 ?? ?? ?? ?? BB C8 0F 00 00", "Results Next", "movss [rsp+0x20],xmm0");
        internal static readonly SigInfo RenderVelvetRoomPromptsSig = new SigInfo("48 8B C4 F3 0F 11 50 ?? F3 0F 11 48 ?? F3 0F 11 40 ?? 53 41 56 41 57", "Velvet Room Prompts", "ret");
        internal static readonly SigInfo RenderChallengeBattlePromptsSig = new SigInfo("48 8B C4 41 57 48 81 EC 30 01 00 00", "Challenge Battle Menu Prompts", "ret");
        internal static readonly SigInfo RenderEventPromptsSig = new SigInfo("48 85 D2 0F 84 ?? ?? ?? ?? 53 55", "Event Prompts", "ret");

        /// <summary>
        /// Signatures that are for everything other than the button prompts
        /// </summary>
        internal static readonly SigInfo[] FunctionSigs = { RenderCursorSig, RenderDateSig, RenderObjectiveSig, RenderMapSig };
        /// <summary>
        /// Signatures for the button prompts (some inline)
        /// </summary>
        internal static readonly SigInfo[] ButtonPromptSigs = {
            RenderSelectionMenuBackSig, RenderSelectionMenuConfirmSig, RenderFieldButtonPromptsSig,
            RenderBattleButtonPromptsSig, RenderBattleBackConfirmSig, RenderBattlePersonaStatsPromptSig,
            RenderCommandSubMenuPromptsSig, RenderPhoneCloseConfirmSig, RenderPhoneScrollBackSig,
            RenderVideoViewingBackConfirmSig, RenderMainMenuPromptsSig, RenderSettingsPromptsSig,
            RenderCraftingPromptsSig, RenderShopBackSig, RenderShopConfirmSig,
            RenderHoldupBreakFormationSig, RenderHoldupTalkSig, RenderHoldupTargetSig,
            RenderHoldupAOASig, RenderGunTargetSig, RenderSkillAimAnalyzeSig, RenderSkillAimTargetSig,
            RenderPersonaDetailsPromptsSig, RenderResultsNextSig, RenderVelvetRoomPromptsSig,
            RenderChallengeBattlePromptsSig, RenderEventPromptsSig
        };
    }

    internal class SigInfo
    {
        /// <summary>
        /// The signature to scan for
        /// </summary>
        internal string Signature { get; }
        /// <summary>
        /// The original code after the call that needs to be added in
        /// </summary>
        internal string OriginalCode { get; }
        /// <summary>
        /// The name of the thing being scanned for
        /// </summary>
        internal string Name { get; }
        /// <summary>
        /// How much to offset the found address by for the hook
        /// </summary>
        internal int Offset { get; }

        internal SigInfo(string signature, string name, string originalCode, int offset = 0)
        {
            Signature = signature;
            Name = name;
            OriginalCode = originalCode;
            Offset = offset;
        }
    }
}
