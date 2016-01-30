�������� �������, ����������� � XML �������:
	Requirements: (������ ���������� ������ � ������ <Requirement>)
		Less					(p1,p2)	- ���������, ���������� true, ���� p1 <  p2.
		LessEqual				(p1,p2)	- ���������, ���������� true, ���� p1 <= p2.
		Equal					(p1,p2)	- ���������, ���������� true, ���� p1 == p2.
		MoreEqual				(p1,p2)	- ���������, ���������� true, ���� p1 >= p2.
		More					(p1,p2)	- ���������, ���������� true, ���� p1 >  p2.
		HasScales				()	- ��������, ���������� true, ���� ������ ���� "���� ������".
	Results: (������ ���������� ������ � ������ <Result>)
		AddLocalParameter			(p1,p2)	- ������� ��������� �������� � ������ p1 � �� ��������� p2.
		RemoveLocalParameter			(p1)	- ������� ��������� �������� � ������ p1.
		IncreaseLocalParameter			(p1)	- ����������� ��������� �������� � ������ p1 �� �������.
		DecreaseLocalParameter			(p1)	- ��������� ��������� �������� � ������ p1 �� �������.
		AddGlobalParameter			(p1,p2)	- ������� ���������� �������� � ������ p1 � �� ��������� p2.
		RemoveGlobalParameter			(p1)	- ������� ���������� �������� � ������ p1.
		IncreaseGlobalParameter			(p1)	- ����������� ���������� �������� � ������ p1 �� �������.
		DecreaseGlobalParameter			(p1)	- ��������� ���������� �������� � ������ p1 �� �������.
		AddItem					(p1)	- ��������� ������� � ������ p1 � ���������, ��� ������� ��� �����.
		RemoveItem				(p1)	- ������� ������� � ������ p1 �� ���������, ���� �� ��� ����.
		AddTime					(p1)	- ��������� p1 ����� �������.
		RemoveTime				(p1)	- �������� p1 ����� �������.
		
�������� ������:
<Quest name = "">															- ��������� ��� ������. name - �������� ������.
	<Settings>															- ��������� ��� ������ ��������.
		<ShowUnavailableAnswers value = "false"/>										- ���� ��������, ����������/�� ���������� ����������� ������. ����� ����� �������� true/false.
	</Settings>
	<Requirements>															- ��������� ��� ������ ���������� ������. ���� ���������� �� ��������� - ����� �� ��������.
		<Requirement function = "Equal" parameter1 = "gotBlackKey" parameter2 = "1"/>						- ���� ����������, ����� ����� ����� �������� type �� ���������� "and" ��� "or" ��� ���������� ����� ��������� ������ ����������.
		<Requirement type = "or" function = "Equal" parameter1 = "youAreCheater" parameter2 = "1"/>
	</Requirements>
	<Nodes>																- ��������� ��� ������ ����� ������.
		<Node name="������������ �������" imageName="wheelBackground">								- ���� ���� ������, name - �������� �����/������ �� ����. imageName - �������� ����������� ��� ������� �����.
			<Text>														- ��������� ��� ������ ������.
				<Part text="���������?">										- ���� ������, text - ����� ��� ������.
					<Requirements/>											- ���� ���������� ��� ����� ������.
				</Part>
			</Text>
			<Answers>													- ��������� ��� ������ ������.
				<Answer text="���������" imageName="imageName">								- ���� ������, text - ����� �������� ������, imageName - �������� ����������� ��� �������� ������.
					<Pointer>��������</Pointer>									- ����-��������� �� ��������� ���� ������. �������� ������ ��������� � ��������� ������ �� ����� ��� ��������. ���� ��� - ��������� ����� �� ������.
					<Requirements/>
					<Results>											- ��������� ��� ������ ����������.
						<Result function = "IncreaseGlobalParameter" parameter1 = "youAreCheater"/>		- ���� ����������, ��������� ���� �� ��������� �������.
						<Result function = "AddLocalParameter" parameter1 = "local" parameter2 = "0"/>
					</Results>
				</Answer>
			</Answers>
		</Node>
		<Node name="��������">
			<Text>
				<Part text="��������. �������.">
					<Requirements/>
				</Part>
			</Text>
			<Answers>
				<Answer text="���!">
					<Pointer/>
					<Requirements/>
					<Results/>
				</Answer>
			</Answers>
		</Node>
	</Nodes>
</Quest>