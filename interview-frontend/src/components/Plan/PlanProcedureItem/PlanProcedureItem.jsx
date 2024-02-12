import React, { useEffect, useState } from "react";
import ReactSelect from "react-select";
import { useParams } from "react-router-dom";
import { assignUnAssignUserPlanProcedures } from "../../../api/api";
import { AssignmentType } from "../../../api/Constants/AssignmentType";

const PlanProcedureItem = ({ procedure, users, assignedUsers}) => {
    const [selectedUsers, setSelectedUsers] = useState([]);
    const { id } = useParams();

    useEffect(() => {
        let selectedUserOptions = [];
        if(assignedUsers)
        {
            assignedUsers.map((user) => {
                selectedUserOptions.push({label: user.name, value: user.userId})
            })
        }
        setSelectedUsers(selectedUserOptions);
    },[assignedUsers])

    const handleAssignUserToProcedure = async (e) => {
        setSelectedUsers([...selectedUsers,e]);
        await assignUnAssignUserPlanProcedures(
            id,
            procedure.procedureId,
            e.value,
            AssignmentType.Assign,
        )
    };

    const handleUnAssignUserToProcedure = async (e) => {
        let updatedUsers = selectedUsers.filter(
            (user) => user.value !== e.value
        )
        setSelectedUsers(updatedUsers);
        await assignUnAssignUserPlanProcedures(
            id,
            procedure.procedureId,
            e.value,
            AssignmentType.DeAssign,
        )
    };

    const handleUserAssignment = async (users) => {
        const existingUsers = new Set(selectedUsers);
        const assignedUsers = new Set(users);

        const assignUsers = [...assignedUsers].filter((user) => !existingUsers.has(user));
        const deAssignUsers = [...existingUsers].filter((user) => !assignedUsers.has(user));

        if(assignUsers.length > 0){
            handleAssignUserToProcedure(assignUsers[0]);
        }
        else{
            handleUnAssignUserToProcedure(deAssignUsers[0]);
        }

    }

    return (
        <div className="py-2">
            <div>
                {procedure.procedureTitle}
            </div>

            <ReactSelect
                className="mt-2"
                placeholder="Select User to Assign"
                isMulti={true}
                options={users}
                value={selectedUsers}
                onChange={(e) => handleUserAssignment(e)}
            />
        </div>
    );
};

export default PlanProcedureItem;
