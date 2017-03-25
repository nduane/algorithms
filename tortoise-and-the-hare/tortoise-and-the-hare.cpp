#include <iostream>
using namespace std;

// Tortoise and hare algoritm for detecting cycles in data structures.  I
// came upon this algorithm in Kent Dybvig's "The Scheme Programming Language"
// book.

// A simple single linked list node.

struct node
{
	int val;
	node *next;

	node(int val)
	{
		this->val = val;
		this->next = nullptr;
	}
};

// Insertion routine.  Tail is a pointer to the last node's next pointer,
// thus a node**.  In order to update tail we need to pass a pointer to
// it, thus a node***.  Alternatively we could return the new value of
// tail which would mean we'd need one less level of indirection.

void insert_into_list(node*** tail, int val)
{
	**tail = new node(val);
	*tail = &(**tail)->next;
}

// Use the tortoise and hare algorithm to detect if the list has a cycle.
// This algorithm detects a cycle by using two pointers one moving one node
// at a time, the tortoise, and one moving two nodes at a time, the hare.
// If the tortoise and the hare meet then there's a cycle.

bool cycle(node* n)
{
	node* tortoise = n;
	node* hare = n;
	
	while (hare != nullptr)
		{

		// Move tortoise one node ahead.
		
		tortoise = tortoise->next;

		// Move hare two nodes ahead.
		
		hare = hare->next;
		if (hare != nullptr)
			{
			hare = hare->next;

			// If the tortoise and hare meet then we must have a cycle.
		
			if (tortoise == hare)
				break;
			}
		}

	// We break out of the loop when either the hare has hit the
	// end of the list, eg. nullptr, or the hare and tortoise point
	// to the same node.
	
	return (hare == nullptr) ? false : true;
}

int main()
{
	node* head = nullptr;
	node** tail = &head;

	int a[] = {2, 12, -5, 13, 9, 0, 2, 15};

	// Create the list.
	
	for (int v : a)
		insert_into_list(&tail, v);

	cout << "cycle: " << ((cycle(head) == true) ? "true" : "false") << endl;

	// Create cycle.

	*tail = head;

	cout << "cycle: " << ((cycle(head) == true) ? "true" : "false") << endl;

	return 0;
}
