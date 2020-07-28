import { Request, Response } from 'express';
import { container } from 'tsyringe';
import CreateAssignmentService from '@domains/assignment/services/CreateAssignmentService';

export default class CreateAssignmentController {
  public async create(request: Request, response: Response): Promise<Response> {
    const {
      age,
      dependents,
      house,
      income,
      marital_status,
      risk_questions,
      vehicle,
    } = request.body;

    const params = {
      age,
      dependents,
      income,
      marital_status,
      risk_questions,
    };

    if (!!house && Object.keys(house)) {
      Object.assign(params, { house });
    }

    if (!!vehicle && Object.keys(vehicle)) {
      Object.assign(params, { vehicle });
    }

    const createAssignment = container.resolve(CreateAssignmentService);
    const assignment = createAssignment.execute(params);

    return response.json(assignment);
  }
}
