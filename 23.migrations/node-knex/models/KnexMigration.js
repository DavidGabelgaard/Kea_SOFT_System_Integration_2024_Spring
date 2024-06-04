const { Model } = require('objection');

class KnexMigration extends Model {

    static get tableName() {
        return 'knex_migrations';
    }

    static get idColumn() {
        return 'id';
    }


    static get jsonSchema() {
        return {
            type: 'object',
            required: [],

            properties: {
                id: { type: 'number' },
                name: { type: 'string' },
                batch: { type: 'number' },
                migration_time: { type: 'string' },
            
            }
        };
    }
    
}

module.exports = KnexMigration;
