; ModuleID = 'obj/Debug/android/marshal_methods.armeabi-v7a.ll'
source_filename = "obj/Debug/android/marshal_methods.armeabi-v7a.ll"
target datalayout = "e-m:e-p:32:32-Fi8-i64:64-v128:64:128-a:0:32-n32-S64"
target triple = "armv7-unknown-linux-android"


%struct.MonoImage = type opaque

%struct.MonoClass = type opaque

%struct.MarshalMethodsManagedClass = type {
	i32,; uint32_t token
	%struct.MonoClass*; MonoClass* klass
}

%struct.MarshalMethodName = type {
	i64,; uint64_t id
	i8*; char* name
}

%class._JNIEnv = type opaque

%class._jobject = type {
	i8; uint8_t b
}

%class._jclass = type {
	i8; uint8_t b
}

%class._jstring = type {
	i8; uint8_t b
}

%class._jthrowable = type {
	i8; uint8_t b
}

%class._jarray = type {
	i8; uint8_t b
}

%class._jobjectArray = type {
	i8; uint8_t b
}

%class._jbooleanArray = type {
	i8; uint8_t b
}

%class._jbyteArray = type {
	i8; uint8_t b
}

%class._jcharArray = type {
	i8; uint8_t b
}

%class._jshortArray = type {
	i8; uint8_t b
}

%class._jintArray = type {
	i8; uint8_t b
}

%class._jlongArray = type {
	i8; uint8_t b
}

%class._jfloatArray = type {
	i8; uint8_t b
}

%class._jdoubleArray = type {
	i8; uint8_t b
}

; assembly_image_cache
@assembly_image_cache = local_unnamed_addr global [0 x %struct.MonoImage*] zeroinitializer, align 4
; Each entry maps hash of an assembly name to an index into the `assembly_image_cache` array
@assembly_image_cache_hashes = local_unnamed_addr constant [154 x i32] [
	i32 32687329, ; 0: Xamarin.AndroidX.Lifecycle.Runtime => 0x1f2c4e1 => 41
	i32 34715100, ; 1: Xamarin.Google.Guava.ListenableFuture.dll => 0x211b5dc => 65
	i32 39109920, ; 2: Newtonsoft.Json.dll => 0x254c520 => 8
	i32 57263871, ; 3: Xamarin.Forms.Core.dll => 0x369c6ff => 60
	i32 101534019, ; 4: Xamarin.AndroidX.SlidingPaneLayout => 0x60d4943 => 51
	i32 120558881, ; 5: Xamarin.AndroidX.SlidingPaneLayout.dll => 0x72f9521 => 51
	i32 165246403, ; 6: Xamarin.AndroidX.Collection.dll => 0x9d975c3 => 26
	i32 182336117, ; 7: Xamarin.AndroidX.SwipeRefreshLayout.dll => 0xade3a75 => 52
	i32 209399409, ; 8: Xamarin.AndroidX.Browser.dll => 0xc7b2e71 => 24
	i32 230216969, ; 9: Xamarin.AndroidX.Legacy.Support.Core.Utils.dll => 0xdb8d509 => 36
	i32 232815796, ; 10: System.Web.Services => 0xde07cb4 => 75
	i32 278686392, ; 11: Xamarin.AndroidX.Lifecycle.LiveData.dll => 0x109c6ab8 => 40
	i32 280482487, ; 12: Xamarin.AndroidX.Interpolator => 0x10b7d2b7 => 34
	i32 318968648, ; 13: Xamarin.AndroidX.Activity.dll => 0x13031348 => 16
	i32 321597661, ; 14: System.Numerics => 0x132b30dd => 11
	i32 342366114, ; 15: Xamarin.AndroidX.Lifecycle.Common => 0x146817a2 => 38
	i32 442521989, ; 16: Xamarin.Essentials => 0x1a605985 => 59
	i32 450948140, ; 17: Xamarin.AndroidX.Fragment.dll => 0x1ae0ec2c => 33
	i32 465846621, ; 18: mscorlib => 0x1bc4415d => 7
	i32 469710990, ; 19: System.dll => 0x1bff388e => 10
	i32 476646585, ; 20: Xamarin.AndroidX.Interpolator.dll => 0x1c690cb9 => 34
	i32 486930444, ; 21: Xamarin.AndroidX.LocalBroadcastManager.dll => 0x1d05f80c => 45
	i32 526420162, ; 22: System.Transactions.dll => 0x1f6088c2 => 69
	i32 605376203, ; 23: System.IO.Compression.FileSystem => 0x24154ecb => 73
	i32 627609679, ; 24: Xamarin.AndroidX.CustomView => 0x2568904f => 30
	i32 663517072, ; 25: Xamarin.AndroidX.VersionedParcelable => 0x278c7790 => 56
	i32 666292255, ; 26: Xamarin.AndroidX.Arch.Core.Common.dll => 0x27b6d01f => 21
	i32 690569205, ; 27: System.Xml.Linq.dll => 0x29293ff5 => 15
	i32 775507847, ; 28: System.IO.Compression => 0x2e394f87 => 72
	i32 809851609, ; 29: System.Drawing.Common.dll => 0x30455ad9 => 71
	i32 843511501, ; 30: Xamarin.AndroidX.Print => 0x3246f6cd => 48
	i32 928116545, ; 31: Xamarin.Google.Guava.ListenableFuture => 0x3751ef41 => 65
	i32 955402788, ; 32: Newtonsoft.Json => 0x38f24a24 => 8
	i32 967690846, ; 33: Xamarin.AndroidX.Lifecycle.Common.dll => 0x39adca5e => 38
	i32 974778368, ; 34: FormsViewGroup.dll => 0x3a19f000 => 3
	i32 1012816738, ; 35: Xamarin.AndroidX.SavedState.dll => 0x3c5e5b62 => 50
	i32 1035644815, ; 36: Xamarin.AndroidX.AppCompat => 0x3dbaaf8f => 20
	i32 1042160112, ; 37: Xamarin.Forms.Platform.dll => 0x3e1e19f0 => 62
	i32 1052210849, ; 38: Xamarin.AndroidX.Lifecycle.ViewModel.dll => 0x3eb776a1 => 42
	i32 1098259244, ; 39: System => 0x41761b2c => 10
	i32 1162731096, ; 40: Luqmit3ish.Android => 0x454dde58 => 0
	i32 1175144683, ; 41: Xamarin.AndroidX.VectorDrawable.Animated => 0x460b48eb => 54
	i32 1204270330, ; 42: Xamarin.AndroidX.Arch.Core.Common => 0x47c7b4fa => 21
	i32 1267360935, ; 43: Xamarin.AndroidX.VectorDrawable => 0x4b8a64a7 => 55
	i32 1293217323, ; 44: Xamarin.AndroidX.DrawerLayout.dll => 0x4d14ee2b => 32
	i32 1365406463, ; 45: System.ServiceModel.Internals.dll => 0x516272ff => 66
	i32 1376866003, ; 46: Xamarin.AndroidX.SavedState => 0x52114ed3 => 50
	i32 1395857551, ; 47: Xamarin.AndroidX.Media.dll => 0x5333188f => 46
	i32 1406073936, ; 48: Xamarin.AndroidX.CoordinatorLayout => 0x53cefc50 => 27
	i32 1460219004, ; 49: Xamarin.Forms.Xaml => 0x57092c7c => 63
	i32 1462112819, ; 50: System.IO.Compression.dll => 0x57261233 => 72
	i32 1469204771, ; 51: Xamarin.AndroidX.AppCompat.AppCompatResources => 0x57924923 => 19
	i32 1582372066, ; 52: Xamarin.AndroidX.DocumentFile.dll => 0x5e5114e2 => 31
	i32 1591627986, ; 53: Luqmit3ish.dll => 0x5ede50d2 => 5
	i32 1592978981, ; 54: System.Runtime.Serialization.dll => 0x5ef2ee25 => 2
	i32 1622152042, ; 55: Xamarin.AndroidX.Loader.dll => 0x60b0136a => 44
	i32 1624863272, ; 56: Xamarin.AndroidX.ViewPager2 => 0x60d97228 => 58
	i32 1636350590, ; 57: Xamarin.AndroidX.CursorAdapter => 0x6188ba7e => 29
	i32 1639515021, ; 58: System.Net.Http.dll => 0x61b9038d => 1
	i32 1657153582, ; 59: System.Runtime => 0x62c6282e => 13
	i32 1658251792, ; 60: Xamarin.Google.Android.Material.dll => 0x62d6ea10 => 64
	i32 1729485958, ; 61: Xamarin.AndroidX.CardView.dll => 0x6715dc86 => 25
	i32 1766324549, ; 62: Xamarin.AndroidX.SwipeRefreshLayout => 0x6947f945 => 52
	i32 1776026572, ; 63: System.Core.dll => 0x69dc03cc => 9
	i32 1788241197, ; 64: Xamarin.AndroidX.Fragment => 0x6a96652d => 33
	i32 1808609942, ; 65: Xamarin.AndroidX.Loader => 0x6bcd3296 => 44
	i32 1813201214, ; 66: Xamarin.Google.Android.Material => 0x6c13413e => 64
	i32 1867746548, ; 67: Xamarin.Essentials.dll => 0x6f538cf4 => 59
	i32 1878053835, ; 68: Xamarin.Forms.Xaml.dll => 0x6ff0d3cb => 63
	i32 1885316902, ; 69: Xamarin.AndroidX.Arch.Core.Runtime.dll => 0x705fa726 => 22
	i32 1919157823, ; 70: Xamarin.AndroidX.MultiDex.dll => 0x7264063f => 47
	i32 2019465201, ; 71: Xamarin.AndroidX.Lifecycle.ViewModel => 0x785e97f1 => 42
	i32 2055257422, ; 72: Xamarin.AndroidX.Lifecycle.LiveData.Core.dll => 0x7a80bd4e => 39
	i32 2079903147, ; 73: System.Runtime.dll => 0x7bf8cdab => 13
	i32 2090596640, ; 74: System.Numerics.Vectors => 0x7c9bf920 => 12
	i32 2097448633, ; 75: Xamarin.AndroidX.Legacy.Support.Core.UI => 0x7d0486b9 => 35
	i32 2126786730, ; 76: Xamarin.Forms.Platform.Android => 0x7ec430aa => 61
	i32 2201231467, ; 77: System.Net.Http => 0x8334206b => 1
	i32 2217644978, ; 78: Xamarin.AndroidX.VectorDrawable.Animated.dll => 0x842e93b2 => 54
	i32 2244775296, ; 79: Xamarin.AndroidX.LocalBroadcastManager => 0x85cc8d80 => 45
	i32 2256548716, ; 80: Xamarin.AndroidX.MultiDex => 0x8680336c => 47
	i32 2261435625, ; 81: Xamarin.AndroidX.Legacy.Support.V4.dll => 0x86cac4e9 => 37
	i32 2279755925, ; 82: Xamarin.AndroidX.RecyclerView.dll => 0x87e25095 => 49
	i32 2315684594, ; 83: Xamarin.AndroidX.Annotation.dll => 0x8a068af2 => 17
	i32 2471841756, ; 84: netstandard.dll => 0x93554fdc => 67
	i32 2475788418, ; 85: Java.Interop.dll => 0x93918882 => 4
	i32 2501346920, ; 86: System.Data.DataSetExtensions => 0x95178668 => 70
	i32 2505896520, ; 87: Xamarin.AndroidX.Lifecycle.Runtime.dll => 0x955cf248 => 41
	i32 2581819634, ; 88: Xamarin.AndroidX.VectorDrawable.dll => 0x99e370f2 => 55
	i32 2620871830, ; 89: Xamarin.AndroidX.CursorAdapter.dll => 0x9c375496 => 29
	i32 2633051222, ; 90: Xamarin.AndroidX.Lifecycle.LiveData => 0x9cf12c56 => 40
	i32 2732626843, ; 91: Xamarin.AndroidX.Activity => 0xa2e0939b => 16
	i32 2737747696, ; 92: Xamarin.AndroidX.AppCompat.AppCompatResources.dll => 0xa32eb6f0 => 19
	i32 2766581644, ; 93: Xamarin.Forms.Core => 0xa4e6af8c => 60
	i32 2778768386, ; 94: Xamarin.AndroidX.ViewPager.dll => 0xa5a0a402 => 57
	i32 2810250172, ; 95: Xamarin.AndroidX.CoordinatorLayout.dll => 0xa78103bc => 27
	i32 2811919066, ; 96: Luqmit3ish => 0xa79a7ada => 5
	i32 2819470561, ; 97: System.Xml.dll => 0xa80db4e1 => 14
	i32 2853208004, ; 98: Xamarin.AndroidX.ViewPager => 0xaa107fc4 => 57
	i32 2855708567, ; 99: Xamarin.AndroidX.Transition => 0xaa36a797 => 53
	i32 2903344695, ; 100: System.ComponentModel.Composition => 0xad0d8637 => 74
	i32 2905242038, ; 101: mscorlib.dll => 0xad2a79b6 => 7
	i32 2916838712, ; 102: Xamarin.AndroidX.ViewPager2.dll => 0xaddb6d38 => 58
	i32 2919462931, ; 103: System.Numerics.Vectors.dll => 0xae037813 => 12
	i32 2921128767, ; 104: Xamarin.AndroidX.Annotation.Experimental.dll => 0xae1ce33f => 18
	i32 2978675010, ; 105: Xamarin.AndroidX.DrawerLayout => 0xb18af942 => 32
	i32 3024354802, ; 106: Xamarin.AndroidX.Legacy.Support.Core.Utils => 0xb443fdf2 => 36
	i32 3044182254, ; 107: FormsViewGroup => 0xb57288ee => 3
	i32 3111772706, ; 108: System.Runtime.Serialization => 0xb979e222 => 2
	i32 3204380047, ; 109: System.Data.dll => 0xbefef58f => 68
	i32 3211777861, ; 110: Xamarin.AndroidX.DocumentFile => 0xbf6fd745 => 31
	i32 3247949154, ; 111: Mono.Security => 0xc197c562 => 76
	i32 3258312781, ; 112: Xamarin.AndroidX.CardView => 0xc235e84d => 25
	i32 3267021929, ; 113: Xamarin.AndroidX.AsyncLayoutInflater => 0xc2bacc69 => 23
	i32 3317135071, ; 114: Xamarin.AndroidX.CustomView.dll => 0xc5b776df => 30
	i32 3317144872, ; 115: System.Data => 0xc5b79d28 => 68
	i32 3340431453, ; 116: Xamarin.AndroidX.Arch.Core.Runtime => 0xc71af05d => 22
	i32 3353484488, ; 117: Xamarin.AndroidX.Legacy.Support.Core.UI.dll => 0xc7e21cc8 => 35
	i32 3362522851, ; 118: Xamarin.AndroidX.Core => 0xc86c06e3 => 28
	i32 3366347497, ; 119: Java.Interop => 0xc8a662e9 => 4
	i32 3374999561, ; 120: Xamarin.AndroidX.RecyclerView => 0xc92a6809 => 49
	i32 3404865022, ; 121: System.ServiceModel.Internals => 0xcaf21dfe => 66
	i32 3429136800, ; 122: System.Xml => 0xcc6479a0 => 14
	i32 3430777524, ; 123: netstandard => 0xcc7d82b4 => 67
	i32 3476120550, ; 124: Mono.Android => 0xcf3163e6 => 6
	i32 3486566296, ; 125: System.Transactions => 0xcfd0c798 => 69
	i32 3501239056, ; 126: Xamarin.AndroidX.AsyncLayoutInflater.dll => 0xd0b0ab10 => 23
	i32 3509114376, ; 127: System.Xml.Linq => 0xd128d608 => 15
	i32 3536029504, ; 128: Xamarin.Forms.Platform.Android.dll => 0xd2c38740 => 61
	i32 3567349600, ; 129: System.ComponentModel.Composition.dll => 0xd4a16f60 => 74
	i32 3627220390, ; 130: Xamarin.AndroidX.Print.dll => 0xd832fda6 => 48
	i32 3632359727, ; 131: Xamarin.Forms.Platform => 0xd881692f => 62
	i32 3633644679, ; 132: Xamarin.AndroidX.Annotation.Experimental => 0xd8950487 => 18
	i32 3641597786, ; 133: Xamarin.AndroidX.Lifecycle.LiveData.Core => 0xd90e5f5a => 39
	i32 3672681054, ; 134: Mono.Android.dll => 0xdae8aa5e => 6
	i32 3676310014, ; 135: System.Web.Services.dll => 0xdb2009fe => 75
	i32 3682565725, ; 136: Xamarin.AndroidX.Browser => 0xdb7f7e5d => 24
	i32 3689375977, ; 137: System.Drawing.Common => 0xdbe768e9 => 71
	i32 3718780102, ; 138: Xamarin.AndroidX.Annotation => 0xdda814c6 => 17
	i32 3758932259, ; 139: Xamarin.AndroidX.Legacy.Support.V4 => 0xe00cc123 => 37
	i32 3786282454, ; 140: Xamarin.AndroidX.Collection => 0xe1ae15d6 => 26
	i32 3822602673, ; 141: Xamarin.AndroidX.Media => 0xe3d849b1 => 46
	i32 3829621856, ; 142: System.Numerics.dll => 0xe4436460 => 11
	i32 3885922214, ; 143: Xamarin.AndroidX.Transition.dll => 0xe79e77a6 => 53
	i32 3896760992, ; 144: Xamarin.AndroidX.Core.dll => 0xe843daa0 => 28
	i32 3920810846, ; 145: System.IO.Compression.FileSystem.dll => 0xe9b2d35e => 73
	i32 3921031405, ; 146: Xamarin.AndroidX.VersionedParcelable.dll => 0xe9b630ed => 56
	i32 3945713374, ; 147: System.Data.DataSetExtensions.dll => 0xeb2ecede => 70
	i32 3955647286, ; 148: Xamarin.AndroidX.AppCompat.dll => 0xebc66336 => 20
	i32 4082807631, ; 149: Luqmit3ish.Android.dll => 0xf35ab34f => 0
	i32 4105002889, ; 150: Mono.Security.dll => 0xf4ad5f89 => 76
	i32 4151237749, ; 151: System.Core => 0xf76edc75 => 9
	i32 4182413190, ; 152: Xamarin.AndroidX.Lifecycle.ViewModelSavedState.dll => 0xf94a8f86 => 43
	i32 4292120959 ; 153: Xamarin.AndroidX.Lifecycle.ViewModelSavedState => 0xffd4917f => 43
], align 4
@assembly_image_cache_indices = local_unnamed_addr constant [154 x i32] [
	i32 41, i32 65, i32 8, i32 60, i32 51, i32 51, i32 26, i32 52, ; 0..7
	i32 24, i32 36, i32 75, i32 40, i32 34, i32 16, i32 11, i32 38, ; 8..15
	i32 59, i32 33, i32 7, i32 10, i32 34, i32 45, i32 69, i32 73, ; 16..23
	i32 30, i32 56, i32 21, i32 15, i32 72, i32 71, i32 48, i32 65, ; 24..31
	i32 8, i32 38, i32 3, i32 50, i32 20, i32 62, i32 42, i32 10, ; 32..39
	i32 0, i32 54, i32 21, i32 55, i32 32, i32 66, i32 50, i32 46, ; 40..47
	i32 27, i32 63, i32 72, i32 19, i32 31, i32 5, i32 2, i32 44, ; 48..55
	i32 58, i32 29, i32 1, i32 13, i32 64, i32 25, i32 52, i32 9, ; 56..63
	i32 33, i32 44, i32 64, i32 59, i32 63, i32 22, i32 47, i32 42, ; 64..71
	i32 39, i32 13, i32 12, i32 35, i32 61, i32 1, i32 54, i32 45, ; 72..79
	i32 47, i32 37, i32 49, i32 17, i32 67, i32 4, i32 70, i32 41, ; 80..87
	i32 55, i32 29, i32 40, i32 16, i32 19, i32 60, i32 57, i32 27, ; 88..95
	i32 5, i32 14, i32 57, i32 53, i32 74, i32 7, i32 58, i32 12, ; 96..103
	i32 18, i32 32, i32 36, i32 3, i32 2, i32 68, i32 31, i32 76, ; 104..111
	i32 25, i32 23, i32 30, i32 68, i32 22, i32 35, i32 28, i32 4, ; 112..119
	i32 49, i32 66, i32 14, i32 67, i32 6, i32 69, i32 23, i32 15, ; 120..127
	i32 61, i32 74, i32 48, i32 62, i32 18, i32 39, i32 6, i32 75, ; 128..135
	i32 24, i32 71, i32 17, i32 37, i32 26, i32 46, i32 11, i32 53, ; 136..143
	i32 28, i32 73, i32 56, i32 70, i32 20, i32 0, i32 76, i32 9, ; 144..151
	i32 43, i32 43 ; 152..153
], align 4

@marshal_methods_number_of_classes = local_unnamed_addr constant i32 0, align 4

; marshal_methods_class_cache
@marshal_methods_class_cache = global [0 x %struct.MarshalMethodsManagedClass] [
], align 4; end of 'marshal_methods_class_cache' array


@get_function_pointer = internal unnamed_addr global void (i32, i32, i32, i8**)* null, align 4

; Function attributes: "frame-pointer"="all" "min-legal-vector-width"="0" mustprogress nofree norecurse nosync "no-trapping-math"="true" nounwind sspstrong "stack-protector-buffer-size"="8" "target-cpu"="generic" "target-features"="+armv7-a,+d32,+dsp,+fp64,+neon,+thumb-mode,+vfp2,+vfp2sp,+vfp3,+vfp3d16,+vfp3d16sp,+vfp3sp,-aes,-fp-armv8,-fp-armv8d16,-fp-armv8d16sp,-fp-armv8sp,-fp16,-fp16fml,-fullfp16,-sha2,-vfp4,-vfp4d16,-vfp4d16sp,-vfp4sp" uwtable willreturn writeonly
define void @xamarin_app_init (void (i32, i32, i32, i8**)* %fn) local_unnamed_addr #0
{
	store void (i32, i32, i32, i8**)* %fn, void (i32, i32, i32, i8**)** @get_function_pointer, align 4
	ret void
}

; Names of classes in which marshal methods reside
@mm_class_names = local_unnamed_addr constant [0 x i8*] zeroinitializer, align 4
@__MarshalMethodName_name.0 = internal constant [1 x i8] c"\00", align 1

; mm_method_names
@mm_method_names = local_unnamed_addr constant [1 x %struct.MarshalMethodName] [
	; 0
	%struct.MarshalMethodName {
		i64 0, ; id 0x0; name: 
		i8* getelementptr inbounds ([1 x i8], [1 x i8]* @__MarshalMethodName_name.0, i32 0, i32 0); name
	}
], align 8; end of 'mm_method_names' array


attributes #0 = { "min-legal-vector-width"="0" mustprogress nofree norecurse nosync "no-trapping-math"="true" nounwind sspstrong "stack-protector-buffer-size"="8" uwtable willreturn writeonly "frame-pointer"="all" "target-cpu"="generic" "target-features"="+armv7-a,+d32,+dsp,+fp64,+neon,+thumb-mode,+vfp2,+vfp2sp,+vfp3,+vfp3d16,+vfp3d16sp,+vfp3sp,-aes,-fp-armv8,-fp-armv8d16,-fp-armv8d16sp,-fp-armv8sp,-fp16,-fp16fml,-fullfp16,-sha2,-vfp4,-vfp4d16,-vfp4d16sp,-vfp4sp" }
attributes #1 = { "min-legal-vector-width"="0" mustprogress "no-trapping-math"="true" nounwind sspstrong "stack-protector-buffer-size"="8" uwtable "frame-pointer"="all" "target-cpu"="generic" "target-features"="+armv7-a,+d32,+dsp,+fp64,+neon,+thumb-mode,+vfp2,+vfp2sp,+vfp3,+vfp3d16,+vfp3d16sp,+vfp3sp,-aes,-fp-armv8,-fp-armv8d16,-fp-armv8d16sp,-fp-armv8sp,-fp16,-fp16fml,-fullfp16,-sha2,-vfp4,-vfp4d16,-vfp4d16sp,-vfp4sp" }
attributes #2 = { nounwind }

!llvm.module.flags = !{!0, !1, !2}
!llvm.ident = !{!3}
!0 = !{i32 1, !"wchar_size", i32 4}
!1 = !{i32 7, !"PIC Level", i32 2}
!2 = !{i32 1, !"min_enum_size", i32 4}
!3 = !{!"Xamarin.Android remotes/origin/d17-5 @ 797e2e13d1706ace607da43703769c5a55c4de60"}
!llvm.linker.options = !{}
