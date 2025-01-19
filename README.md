# DiLib
손 쉬운 Dependency Injection을 위한 프로젝트입니다.

시작한 이유는 스스로가 쓰기에 불편함이 없고, 충분히 빠른 DI를 만들기 위해서 입니다. 프레임워크를 목표로 하는 것은 아니고, 간소한 형태를 고려하고 있습니다.

솔직히 잘 모르는 것을 잘 모르는 채로 하고 있다는 느낌이 듭니다.

실제로 사용하기에 앞서, 개선할 점과 정리할 점을 검토해볼 필요가 있으며, 퍼포먼스를 점검해볼 필요도 있습니다.
아직까지 실제로 사용하기에 부족한 것이 명확해 보입니다.

한 DI Framework을 사용해봤던 경험에 따르면 Reflection을 사용해서 지정된 Attribute을 가진 필드를 탐색하는데에 적지않은 CPU 자원이 소요되었습니다.
따라서, Attribute는 편리하지만 포함하지 않으려고 합니다.

참고한 프로젝트에서는 Reflection을 사용하지 않는 가장 간단한 Resolve시 다음과 같이 동작했습니다
(설명을 위해 간소화한 것으로 실제 동작과는 조금 다릅니다.).
1. Resolve\<T\> 메서드의 제네릭 인자에 대해서 typeof(T) 연산자를 이용하여 Type형 변수를 얻음.
2. Dictionary\<Type, object\>형 필드에서 1에서 얻은 변수를 키로 이용하여 검색.
3. object를 정적 캐스팅을 하여 반환.

여기서 제네릭을 조금 다르게 이용해서 정적 캐스팅 부분을 제거해봤습니다. 간단하게 설명하면
(설명을 위해 간소화한 것으로 실제 동작과는 조금 다릅니다.), 
1. InstanceAccessor\<Type\> 클래스를 정의.
2. Get\<Type\>(DiContext)로 조회.
3. static Dictionary\<DiContext, Type\> _dictionary에서 검색하여 반환.

제네릭 클래스의 정적 필드를 이용해서 정적 캐스팅을 대체하는 방법에 대해서 생각해본 결과입니다만, 실제로 작성해보고 실효성이 없다는 것을 확인할 수 있었습니다.

Package Manager에서 샘플 중 'UsageSample'을 임포트하여 쉬운 사용법을 확인할 수 있습니다.

## 참고로한 DI 프레임워크
Reflex by gustavopsantos -- https://github.com/gustavopsantos/Reflex -- License: MIT  

## 설치 방법
### 1. 설치
'Package Manager'의 좌상단의 '+' 버튼을 누르고, 'Add package from git URL'을 누르고, 다음 주소를 입력하세요.
`https://github.com/Feverfew826/DiLib.git?path=Assets/Plugins/DiLib`