# DiLib
손 쉬운 Dependency Injection을 위한 프로젝트입니다.

시작한 이유는 스스로가 쓰기에 불편함이 없고, 가벼운 DI 도구를 만들기 위해서 입니다. 프레임워크를 목표로 하는 것은 아니고, 간소하게 만들어 손쉽게 사용할 수 있는 형태를 고려하고 있습니다.

복잡한 프레임워크는 작은 프로젝트에는 어울리지 않고, 첫 사용 시 읽을 문서가 많아 진입 장벽이 있으며, 싱글톤을 기반으로 개발해온 프로그래머에게는 다소 귀찮게 느껴질 것입니다.
나름대로 복잡한 개념은 제거하고, 핵심만 남겨서 구현하기 위해 노력했습니다.\

## 사용법
기본적인 사용법은 간단합니다.

`DiLib.Containers.ProjectContext`, `DiLib.Containers.SceneContext(Scene)` 를 호출하면, `IDependencyContainer` 를 반환합니다.

이 `IDependencyContainer`에 `Set<T>()`를 이용하여 의존성을 준비하고, `Get<T>()`를 호출하여 의존성을 가져다 사용할 수 있습니다(의존성은 인스턴스로 준비할 수도 있고, 메서드로 준비할 수도 있습니다.).

`ProjectContext`는 게임 실행 시간 전체에 걸쳐서 유지되는 전역적인 문맥입니다.

`SceneContext`는 씬 플레이 동안만 사용되고 씬이 언로드 될 때 파기되는 문맥입니다.

`ProjectContext`와 `SceneContext`만으로는 부족할지도 모릅니다. 그럴 때는 `IDependencyContainer.Child<Key>(Key)`를 호출하여 목적과 용도에 따라 문맥을 세분화하여 확장해 갈 수 있습니다(Key는 enum 이나 string 등을 자유롭게 사용하는 것을 고려하였습니다.).

부모를 파기(Dispose)할 때는 그 자식도 파기합니다. 참고로 `ProjectContext`와 `SceneContext`도 부모-자식 관계입니다.

### 편의 기능
가끔 타이틀 씬으로 되돌아가 모든 것을 파기하고 재시작 해야 할 때가 있습니다. 그럴 때를 위해 `DiLib.Containers.ForceToDisposeProjectContextContainer`로 `ProjectContext`를 강제로 파기하고 재생성할 수 있도록 해뒀습니다(개념이 다소 흔들릴 수도 있지만 그 이상으로 편리하다고 생각했습니다.).

문맥 파기와 생명 주기를 함께 하고 싶은 `System.IDisposable`객체가 있다면, 확장 메서드 `AddTo<T>(this T, IDependencyContainer)`를 이용해서 함께 파기되도록 등록 해둘 수 있습니다. 참고로 파기는 등록의 역순입니다.

문맥이 생성될 때, 로그를 남기는 기능이 있습니다. `DiLib.Auditor.EnableLogging(bool)`을 통해 로그를 끌 수 있습니다.

SceneInjectionManager 컴포넌트와 IInjectable 인터페이스를 이용하면, 씬에 배치된 객체들에 대하여 Awake 시점에 손쉽게 의존성을 주입할 수 있습니다(다만, Object.GetComponent<T>() 계열 메서드로 주입 대상을 찾으므로 퍼포먼스가 나쁠 수 있습니다. 미리 목록을 구성해두는 기능을 만들면 어떨까 합니다만, 자동으로 특정 시점에 해당 기능이 동작하지 않으면 불편할 것 같네요.).


## 적용에 앞서서...
개선할 점과 정리할 점을 검토해볼 필요가 있으며, 편의 기능들에 대한 퍼포먼스를 점검해볼 필요도 있습니다.

한 DI Framework을 사용해봤던 경험에 따르면 Reflection을 사용해서 지정된 Attribute을 가진 필드를 탐색하는데에 적지않은 CPU 자원이 소요되었습니다.
따라서, Attribute는 편리하지만 포함하지 않습니다.

Package Manager에서 샘플 중 'UsageSample'을 임포트하여 가장 기본적인 사용법을 확인할 수 있습니다.
Package Manager에서 샘플 중 'SceneInjectionManagerSample'을 임포트하여, 씬에 배치된 객체들에 의존성 주입 시 편의 기능 사용법을 확인할 수 있습니다.

## 참고로한 DI 프레임워크
Reflex by gustavopsantos -- https://github.com/gustavopsantos/Reflex -- License: MIT  

## 설치 방법
### 1. 설치
'Package Manager'의 좌상단의 '+' 버튼을 누르고, 'Add package from git URL'을 누르고, 다음 주소를 입력하세요.
`https://github.com/Feverfew826/DiLib.git?path=Assets/Plugins/DiLib`
