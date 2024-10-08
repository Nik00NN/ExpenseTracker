import { ActionIcon, Button, Group, Table, Title } from '@mantine/core';
import { useDeleteCategoryMutation, useGetCategoriesQuery } from '../../state/category/api';
import { IconEdit, IconTrash } from '@tabler/icons-react';
import { Link } from 'react-router-dom';
import { notifications } from '@mantine/notifications';
const Transactions = () => {
	const { data: categories = [], isLoading: isLoadingCategories } = useGetCategoriesQuery();
	const [deleteCategory] = useDeleteCategoryMutation();

	return (
		<div>
			<Group justify='space-between'>
				<Title>Transactions</Title>
				<Link to='/category/new'>
					<Button>Add</Button>
				</Link>
			</Group>

			{isLoadingCategories && <p>loading...</p>}

			<Table striped highlightOnHover>
				<Table.Thead>
					<Table.Tr>
						<Table.Th>Id</Table.Th>
						<Table.Th>Name</Table.Th>
						<Table.Th>Actions</Table.Th>
					</Table.Tr>
				</Table.Thead>
				<Table.Tbody>
					{categories.map((category) => (
						<Table.Tr key={category.id}>
							<Table.Td>
								<Link to={`/categories/${category.id}`}>{category.id}</Link>
							</Table.Td>
							<Table.Td>{category.categoryName}</Table.Td>
							<Table.Td>
								<ActionIcon
									onClick={async () => {
										const result = await deleteCategory(category.id);
										notifications.show({
											title: 'Category Deleted',
											message: `Category ${category.id} has been deleted!`,
											position: 'bottom-right',
										});
										console.log(result);
									}}
								>
									<IconTrash size={17} />
								</ActionIcon>
								<ActionIcon
									onClick={async () => {
										const result = await deleteCategory(category.id);
										notifications.show({
											title: 'Category Deleted',
											message: `Category ${category.id} has been deleted!`,
											position: 'bottom-right',
										});
										console.log(result);
									}}
								>
									<IconEdit size={17} />
								</ActionIcon>
							</Table.Td>
						</Table.Tr>
					))}
				</Table.Tbody>
			</Table>
		</div>
	);
};

export default Transactions;
