Описание функций, действующих в XML квестах:
	Requirements: (Должны вызываться только в блоках <Requirement>)
		Less					(p1,p2)	- сравнение, возвращает true, если p1 <  p2.
		LessEqual				(p1,p2)	- сравнение, возвращает true, если p1 <= p2.
		Equal					(p1,p2)	- сравнение, возвращает true, если p1 == p2.
		MoreEqual				(p1,p2)	- сравнение, возвращает true, если p1 >= p2.
		More					(p1,p2)	- сравнение, возвращает true, если p1 >  p2.
		HasScales				()	- проверка, возвращает true, если собран знак "весы судьбы".
	Results: (Должны вызываться только в блоках <Result>)
		AddLocalParameter			(p1,p2)	- создает локальный параметр с именем p1 и со значением p2.
		RemoveLocalParameter			(p1)	- удаляет локальный параметр с именем p1.
		IncreaseLocalParameter			(p1)	- увеличивает локальный параметр с именем p1 на единицу.
		DecreaseLocalParameter			(p1)	- уменьшает локальный параметр с именем p1 на единицу.
		AddGlobalParameter			(p1,p2)	- создает глобальный параметр с именем p1 и со значением p2.
		RemoveGlobalParameter			(p1)	- удаляет глобальный параметр с именем p1.
		IncreaseGlobalParameter			(p1)	- увеличивает глобальный параметр с именем p1 на единицу.
		DecreaseGlobalParameter			(p1)	- уменьшает глобальный параметр с именем p1 на единицу.
		AddItem					(p1)	- добавляет предмет с именем p1 в инвентарь, при наличии там места.
		RemoveItem				(p1)	- удаляет предмет с именем p1 из инвентаря, если он там есть.
		AddTime					(p1)	- добавляет p1 минут времени.
		RemoveTime				(p1)	- отнимает p1 минут времени.
		
Описание блоков:
<Quest name = "">															- контейнер для блоков. name - название квеста.
	<Settings>															- контейнер для блоков настроек.
		<ShowUnavailableAnswers value = "false"/>										- блок настроек, показывать/не показывать недоступные ответы. может иметь значения true/false.
	</Settings>
	<Requirements>															- контейнер для блоков требований квеста. если требования не выполнены - квест не начнется.
		<Requirement function = "Equal" parameter1 = "gotBlackKey" parameter2 = "1"/>						- блок требований, может иметь иметь параметр type со значениями "and" или "or" для логической связи различных блоков требований.
		<Requirement type = "or" function = "Equal" parameter1 = "youAreCheater" parameter2 = "1"/>
	</Requirements>
	<Nodes>																- контейнер для блоков узлов квеста.
		<Node name="Сокровищница времени" imageName="wheelBackground">								- блок узла квеста, name - название блока/ссылка на блок. imageName - название изображения для данного блока.
			<Text>														- контейнер для блоков текста.
				<Part text="считерить?">										- блок текста, text - текст для показа.
					<Requirements/>											- блок требований для блока текста.
				</Part>
			</Text>
			<Answers>													- контейнер для блоков ответа.
				<Answer text="считерить" imageName="imageName">								- блок ответа, text - текст варианта ответа, imageName - название изображения для варианта ответа.
					<Pointer>считерил</Pointer>									- блок-указатель на следующий узел квеста. значение должно совпадать с названием одного из узлов для перехода. если нет - проиходит выход из квеста.
					<Requirements/>
					<Results>											- контейнер для блоков результата.
						<Result function = "IncreaseGlobalParameter" parameter1 = "youAreCheater"/>		- блок результата, применяет одну из описанных функций.
						<Result function = "AddLocalParameter" parameter1 = "local" parameter2 = "0"/>
					</Results>
				</Answer>
			</Answers>
		</Node>
		<Node name="считерил">
			<Text>
				<Part text="считерил. молодец.">
					<Requirements/>
				</Part>
			</Text>
			<Answers>
				<Answer text="ура!">
					<Pointer/>
					<Requirements/>
					<Results/>
				</Answer>
			</Answers>
		</Node>
	</Nodes>
</Quest>